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
//
// 9 = monster spawner
// 10 = storage box
//////////////////////////////////

public class LevelGenerator : MonoBehaviour 
{
	public NavMeshSurface navMesh;

	public const int MinSize1 = 5;
	public const int MinSize2 = 12;

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject breakablePrefab;
	public GameObject exitWallPrefab;
	public GameObject levelEndPrefab;
	public GameObject storageBoxPrefab;

	public GameObject[] treasureWallPrefabs;
	public GameObject monsterSpawnerPrefabs;
	private float bigSpiderChance, armoredSpiderChance, bigArmoredSpiderChance;

	public GameObject playerController;
	public Light globalLight;

	public GameObject floorParent;
	public GameObject wallsParent;
	public GameObject usableObjectParent;
	public GameObject spawnerParent;

	private int mazeSizeX;
	private int mazeSizeZ;
	private float roomSizeFactor;

	private bool shouldPlaceBox = false;

	// 2D array representing the map
	private int[,] mapData;

	// Use this for initialization
	void Start ()
	{
		GameData.maxMonsterPopulation = Mathf.FloorToInt(Mathf.Clamp(0.5f * GameData.level, 0, 20));
		Debug.Log(GameData.maxMonsterPopulation);
		CalculateMosnterChances();

		SetGlobalLight();

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
					GameObject block = CreateChildPrefab(wallPrefab, wallsParent, x, 1, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));

					// create floor below walls
					block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				} 
				else if(mapData[x, z] == 2) //breakable block
				{ 
					GameObject block = CreateChildPrefab(breakablePrefab, wallsParent, x, 1, z);
					block.transform.Rotate(new Vector3(Random.Range(-1, 1) * 90, Random.Range(-1, 1) * 90, Random.Range(-1, 1) * 90));

					// create floor below walls
					block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
				else if(mapData[x, z] >= 6 && mapData[x, z] <= 8) //treasure block
				{ 
					GameObject block = CreateChildPrefab(treasureWallPrefabs[mapData[x, z] - 6], wallsParent, x, 1, z);
					block.transform.Rotate(new Vector3(Random.Range(-1, 1) * 90, Random.Range(-1, 1) * 90, Random.Range(-1, 1) * 90));

					// create floor below walls
					block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
                else if(mapData[x, z] == 0)// floor only
				{ 
					GameObject block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
				else if(mapData[x, z] == 5)// player spawn, has to be set before the exit collider
				{ 
					playerController.transform.position = new Vector3(x, .5f, z);
					GameObject block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
				else if(mapData[x, z] == 3)// level exit collider
				{ 
					GameObject block = CreateChildPrefab(exitWallPrefab, wallsParent, x, 1, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));

					CreateChildPrefab(levelEndPrefab, usableObjectParent, x, 1, z);
				}
				else if(mapData[x, z] == 4)// level exit border
				{ 
					GameObject block = CreateChildPrefab(exitWallPrefab, wallsParent, x, 1, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
				else if(mapData[x, z] == 9)// monster spawner
				{ 
					var thisMonsterSpawner = CreateChildPrefab(monsterSpawnerPrefabs, spawnerParent, x, 1, z);
					thisMonsterSpawner.GetComponent<MonsterSpawner>().SetMonsterType(CalculateMonsterSpawner());
					GameObject block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
				else if(mapData[x, z] == 10)// storage box
				{ 
					CreateChildPrefab(storageBoxPrefab, usableObjectParent, x, .5f, z);
					GameObject block = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
					block.transform.Rotate(new Vector3(0, Random.Range(-1, 2) * 90, 0));
				}
			}
		}

		// Generate NavMesh
		navMesh.BuildNavMesh();

		// Fill map with monsters
		spawnerParent.GetComponent<SpawnTimer>().PopulateMap(playerController.transform);
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	int[,] GenerateMazeData()
	{
		float treasureChance = Mathf.Max(0f, 0.25f * GameData.level + 7.5f);
		float silverChance = Mathf.Max(0f, -1.38f * GameData.level + 101.38f);
		float goldChance = Mathf.Max(0f, 2.5f * GameData.level - 15f);
		float emeraldChance = Mathf.Max(0f, GameData.level - 10f);
		float summedChances = silverChance + goldChance + emeraldChance;

		mazeSizeX = Mathf.Min(GameData.level + MinSize1, 40);
		mazeSizeZ = Mathf.Min(Random.Range(GameData.level + MinSize2, Mathf.FloorToInt(GameData.level * 1.5f) + MinSize2), 60);

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
		int numberOfRooms = Mathf.FloorToInt(Mathf.Pow(GameData.level, 1.5f)/5);

		//prevents dividing for 0
		if (numberOfRooms < 1)
		{
			numberOfRooms = 1;
		}
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

			int startingPointX = (mazeSizeX - 1 > roomDimensionX) ? startingPointX = Random.Range(1, mazeSizeX - 1 - roomDimensionX) : startingPointX = Random.Range(1, mazeSizeX - 1);
			int startingPointZ = (mazeSizeZ - 1 > roomDimensionZ) ? startingPointZ = Random.Range(1, mazeSizeZ - 1 - roomDimensionZ) : startingPointZ = Random.Range(1, mazeSizeZ - 1);

			for(int z = startingPointZ; z < startingPointZ + roomDimensionZ; z++)
			{
				for(int x = startingPointX; x < startingPointX + roomDimensionX; x++)
				{
					if(x > 0 && x < mazeSizeX - 1 && z > 0 && z < mazeSizeZ - 1)
					{
						data[x,z] = 0;
					}
				}
			}

			/////////////////////////////////
			// Generate Monster Spawners
			/////////////////////////////////

			//calculates the room center and place the monster spawner at a correct location
			int centerPointX = Mathf.FloorToInt((startingPointX + roomDimensionX/2));
			int centerPointZ = Mathf.FloorToInt((startingPointZ + roomDimensionZ/2));

			// if room center is a valid point, place the spawner there
			if(centerPointX > 0 && centerPointX < mazeSizeX - 1 && centerPointZ > 0 && centerPointZ < mazeSizeZ - 1)
			{
				data[centerPointX,centerPointZ] = 9;
				// if a box should be place, turns the firts monster spawner of the map into a box
			}

			// if not, put the spawner at the corner
			else
			{
				// if a box should be place, turns the firts monster spawner of the map into a box
				if(shouldPlaceBox)
				{
					shouldPlaceBox = false;
					data[startingPointX, startingPointZ] = 10;
				}
				else
				{
					data[startingPointX, startingPointZ] = 9;
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
					break;
				}
			} while (entryExitDistance < fullMapDistance * .3f || (Mathf.Abs(entryPointX - exitPointX) < 2 && Mathf.Abs(entryPointZ - exitPointZ) < 2));
		
			infiniteLoopBreak--;
			if(infiniteLoopBreak < 0)
			{
				Debug.LogError("Infinite loop while generating level!" + "fullMapDistance: " + fullMapDistance + ", entryExitDistance: " + entryExitDistance + " dX: " + Mathf.Abs(entryPointX - exitPointX) + " dZ: " + Mathf.Abs(entryPointZ - exitPointZ));
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

		// place the storage box
		// there surely a much better way to do this
		if(Random.Range(0f, 100f) < GameData.boxChance)
		{
			shouldPlaceBox = true;
			GameData.boxChance -= 20f;
		}
		else
		{
			shouldPlaceBox = false;
			GameData.boxChance += 15f;
		}

		if(shouldPlaceBox)
		{
			int skip = Random.Range(0, mazeSizeX + mazeSizeZ);

			for(int x = 1; x < mazeSizeX -1; x++)
			{
				for(int z = 1; z < mazeSizeZ -1; z++)
				{
					if(data[x, z] == 0)
					{
						skip--;
					}
					if(skip < 0)
					{
						data[x, z] = 10;
						break;
					}
				}
				if(skip < 0)
				{
					break;
				}
			}
		}
		

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	public GameObject CreateChildPrefab(GameObject prefab, GameObject parent, float x, float y, float z)
	{
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
		return myPrefab;
	}

	private int CalculateMonsterSpawner()
	{
		if(Random.Range(0f, 100f) < bigArmoredSpiderChance)
		{
			return 3;
		}
		else if(Random.Range(0f, 100f) < armoredSpiderChance)
		{
			return 2;
		}
		else if(Random.Range(0f, 100f) < bigSpiderChance)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}

	private void CalculateMosnterChances()
	{
		bigSpiderChance = (-Mathf.Pow(GameData.level - 10f, 2f)/25f + 0.2f) * 100f;

		armoredSpiderChance = ((GameData.level - 10f)/15f) * 100f;
		if(armoredSpiderChance > 60f)
		armoredSpiderChance = 60f;

		bigArmoredSpiderChance = ((GameData.level - 10f)/30f) * 100f;
		if(bigArmoredSpiderChance > 15f)//had to use IF, Mathf.Max was giving weird results
		bigArmoredSpiderChance = 15f;
	}

	private void SetGlobalLight()
	{
		globalLight.intensity = Mathf.Max(0f, 0.30f - GameData.level/75f);
	}
}