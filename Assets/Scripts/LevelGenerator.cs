using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.AI.Navigation;

//////////////////////////////////
// MapData dictionary
// 0 = floor only
// 1 = solid wall
// 2 = breakable wall
//
// 3 = level end (collider)
// 4 = level end (border)
// 5 = level entry
//
// 6 = silver block
// 7 = gold block
// 8 = emerald block
//////////////////////////////////

public class LevelGenerator : MonoBehaviour 
{
	public NavMeshSurface navMesh;

	public const int MinSize1 = 3;
	public const int MinSize2 = 10;

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject breakablePrefab;
	public GameObject levelEndPrefab;

	public GameObject[] treasureWall;
	public GameObject[] monsterSpawner;

	public GameObject playerController;

	public GameObject floorParent;
	public GameObject wallsParent;
	public GameObject usableObjectParent;
	public GameObject spawnerParent;

	private int mazeSizeX;
	private int mazeSizeZ;
	private float roomSizeFactor;

	// 2D array representing the map
	private int[,] mapData;

	// Use this for initialization
	void Start ()
	{
		//define size based on level
		// initialize map 2D array
		mapData = GenerateMazeData();

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSizeZ; z++) 
		{
			for (int x = 0; x < mazeSizeX; x++) 
			{
				if (mapData[x, z] == 1) // solid walls
				{ 
					CreateChildPrefab(wallPrefab, wallsParent, x, 1, z);

					// create floor below walls
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				} 
				else if(mapData[x, z] == 2) //breakable block
				{ 
					CreateChildPrefab(breakablePrefab, wallsParent, x, 1, z);

					// create floor below walls
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				}
				else if(mapData[x, z] >= 6 && mapData[x, z] <= 8) //treasure block
				{ 
					CreateChildPrefab(treasureWall[mapData[x, z] - 6], wallsParent, x, 1, z);

					// create floor below walls
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				}
                else if(mapData[x, z] == 0)// floor only
				{ 
					CreateChildPrefab(monsterSpawner[0], spawnerParent, x, 1, z);
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				}
				else if(mapData[x, z] == 5)// player spawn, has to be set before the exit collider
				{ 
					playerController.transform.position = new Vector3(x, .5f, z);
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				}
				else if(mapData[x, z] == 3)// level exit collider
				{ 
					CreateChildPrefab(breakablePrefab, wallsParent, x, 1, z);
					CreateChildPrefab(levelEndPrefab, usableObjectParent, x, 1, z);
				}
				else if(mapData[x, z] == 4)// level exit border
				{ 
					CreateChildPrefab(breakablePrefab, wallsParent, x, 1, z);
				}

			}
		}

		// Generate NavMesh
		navMesh.BuildNavMesh();
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	int[,] GenerateMazeData()
	{
		float treasureChance = Mathf.Max(0f, 0.25f * GameData.level + 7.5f);
		float silverChance = Mathf.Max(0f, -1.38f * GameData.level + 101.38f);
		float goldChance = Mathf.Max(0f, 2.5f * GameData.level - 25f);
		float emeraldChance = Mathf.Max(0f, GameData.level - 20f);
		float summedChances = silverChance + goldChance + emeraldChance;

		mazeSizeX = Mathf.Min(GameData.level + MinSize1, 30);
		mazeSizeZ = Mathf.Min(Random.Range(GameData.level + MinSize2, Mathf.FloorToInt(GameData.level * 1.5f) + MinSize2), 50);

		roomSizeFactor = Random.Range(1.0f, 3.0f);

		int[,] data = new int[mazeSizeX, mazeSizeZ];

		// Generate base map
        for(int z = 0; z < mazeSizeZ; z++)
        {
            for (int x = 0; x < mazeSizeX; x++)
            {
                if(x == 0 || x == mazeSizeX - 1 || z == 0 || z == mazeSizeZ - 1) // map border
                {
                    data[x,z] = 1;
                }
                else
                {
					// Generate a random treasure block or not, based on chances
					if(Random.Range(0f, 100f) < treasureChance)
					{
						float randiValue = Random.Range(0f, summedChances);
						// Treasure block
						if(randiValue < silverChance)
						{
							//silver block
							data[x,z] = 6;
						}
						else if(randiValue < silverChance + goldChance)
						{
							//gold block
							data[x,z] = 7;
						}
						else
						{
							//emerald block
							data[x,z] = 8;
						}
					}
					else
					{
						// Dirt
						data[x,z] = 2;
					}
                }
            }
        }

		// Generate rooms
		//need to think if its better to keep rounding the number as I go or keep calculating with
		//real numbers and only round it once I record the room dimensions
		//they both will work the same way, but one way will be more efficient

		//calculate the inner area between the outer walls
		int innerTilesAmount = mazeSizeX * mazeSizeZ - 2 * (mazeSizeZ + mazeSizeX) + 4;
		//calculate the amount of tiles that will be removed form the room
		float tilesToRemove = innerTilesAmount * roomSizeFactor;

		//calculate the amount of rooms are going to be made
		int numberOfRooms = Random.Range(3, 9);

		//calculate the base size for the rooms
		float roomBaseSize = tilesToRemove / numberOfRooms;

		for(int room = 0; room < numberOfRooms; room++)
		{
			////////////////////////////////////////////////////////////
			//calculate a random size variation for each room
			//define coordinates to make the room
			//get square root of roomSize
			//multiple the squareRoot by a randomRange (from 0 to 1) to get nonsquare rooms sometimes
			//one of the sides gets the squareRoot*Range and the other gets squareRoot*(1 - Range)
			//this gives me the room dimensions to an aproximate of the calculated area
			//then pick a random starting position to start drawing the room
			//the starting point takes in consideration its max value to respect maze size
			
			float thisRoomSize = Random.Range(1f, 3f) * roomBaseSize;

			float rootedDimensions = Mathf.Sqrt(thisRoomSize);
			float roomFormatFactor = Random.value;

			int roomDimensionX = Mathf.FloorToInt(rootedDimensions * roomFormatFactor);
			int roomDimensionZ = Mathf.FloorToInt(rootedDimensions * (1 - roomFormatFactor));

			int startingPointX = Random.Range(1, mazeSizeX - roomDimensionX);
			int startingPointZ = Random.Range(1, mazeSizeZ - roomDimensionZ);

			for(int z = startingPointZ; z < startingPointZ + roomDimensionZ; z++)
			{
				for(int x = startingPointX; x < startingPointX + roomDimensionX; x++)
				{
					if(x > 0 && x < mazeSizeX - 1 && z > 0 && z < mazeSizeZ - 1)
					{
						data[x,z] = 0;
					}
					else
					{
						Debug.Log("Room is too big!");
					}
				}
			}
		}

		/////////////////////////////////
		// Generate Exit and Entry points
		/////////////////////////////////
		int entryPointX, entryPointZ, exitPointX, exitPointZ;
		
		float entryExitDistance;
		float fullMapDistance = Mathf.Sqrt(Mathf.Pow(mazeSizeX - 2, 2f) + Mathf.Pow(mazeSizeZ - 2, 2f));
		
		int badExitPointBreak;
		int infiniteLoopBreak = 25;
		do
		{
			badExitPointBreak = 25;

			exitPointX = Random.Range(1, mazeSizeX - 1);
			exitPointZ = Random.Range(1, mazeSizeZ - 1);
			// randomly pick a random entry point and check if it's valid
			do
			{
				entryPointX = Random.Range(1, mazeSizeX - 1);
				entryPointZ = Random.Range(1, mazeSizeZ - 1);

				entryExitDistance = Mathf.Sqrt( Mathf.Pow(exitPointX - entryPointX, 2f) + Mathf.Pow(exitPointZ - entryPointZ, 2f));

				badExitPointBreak--;
				if(badExitPointBreak < 0)
				{
					Debug.Log("Bad exit point, replacing it.");
					break;
				}
			} while (entryExitDistance < fullMapDistance * .3f || (Mathf.Abs(entryPointX - exitPointX) < 2 && Mathf.Abs(entryPointZ - exitPointZ) < 2));
		
			infiniteLoopBreak--;
			if(infiniteLoopBreak < 0)
			{
				//TODO: turn this into a throw exception
				Debug.Log("Infinite loop while generating level.");
				Debug.Log("fullMapDistance: " + fullMapDistance + ", entryExitDistance: " + entryExitDistance + " dX: " + Mathf.Abs(entryPointX - exitPointX) + " dZ: " + Mathf.Abs(entryPointZ - exitPointZ));
				break;
			}

		} while(badExitPointBreak < 0);

		// generate exit border
		for(int z = exitPointZ - 1; z <= exitPointZ + 1; z++)
		{
			for(int x = exitPointX - 1; x <= exitPointX + 1; x++)
			{
				if( z > 0 && z < mazeSizeZ - 1  && x > 0 && x < mazeSizeX - 1)
					data[x, z] = 4;
			}
		}
		data[exitPointX, exitPointZ] = 3;

		// create a room at player entry point
		int entryRoomSizeX = Random.Range(1, 5);
		int entryRoomSizeZ = Random.Range(1, 5);
		for(int z = entryPointZ - entryRoomSizeZ; z < entryPointZ + entryRoomSizeZ; z++)
		{
			for(int x = entryPointX - entryRoomSizeX; x < entryPointX + entryRoomSizeX; x++)
			{
				if( z > 0 && z < mazeSizeZ - 1 && x > 0 && x < mazeSizeX - 1 )
				{
					if(data[x,z] != 3 && data[x,z] != 4)
						data[x, z] = 0;
				}
					
			}
		}

		data[entryPointX, entryPointZ] = 5;

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	void CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z)
	{
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
	}
}