using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////
// MapData dictionary
// 0 = floor only
// 1 = solid wall
// 2 = breakable wall
// 3 = level end
//////////////////////////////////

public class LevelGenerator : MonoBehaviour {

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject breakablePrefab;

	public GameObject characterController;

	public GameObject floorParent;
	public GameObject wallsParent;

	public int mazeSizeX;
	public int mazeSizeZ;
	public float percentageToClear;

	// 2D array representing the map
	private int[,] mapData;

	// Use this for initialization
	void Start ()
	{

		// initialize map 2D array
		mapData = GenerateMazeData();

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSizeZ; z++) {
			for (int x = 0; x < mazeSizeX; x++) {
				if (mapData[x, z] == 1) { // solid walls
					CreateChildPrefab(wallPrefab, wallsParent, x, 1, z, 7);

					// create floor below walls
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z, 7);
				} 
				else if(mapData[x, z] == 2) //breakable block
				{ 
					CreateChildPrefab(breakablePrefab, wallsParent, x, 1, z, 8);

					// create floor below walls
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z, 7);
				}
                else if(mapData[x, z] == 0)// floor only
				{ 
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z, 7);
				}
                // TODO: Generate all the other types of tiles
                // elseif(isso){bota isso};

			}
		}
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	int[,] GenerateMazeData()
	{
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
                    data[x,z] = 2;
                }
            }
        }

		// Generate rooms
		//need to think if its better to keep rounding the number as I go or keep calculating with
		//real numbers and only round it once I record the room dimensions
		//they both will work the same way, but one way will be more efficient

		//calculate the inner area between the outer walls
		int innerTilesAmount = mazeSizeX * mazeSizeZ - 2 * (mazeSizeZ + mazeSizeX);
		//calculate the amount of tiles that will be removed form the room
		float tilesToRemove = innerTilesAmount * percentageToClear;

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
					if(x != 0 && x != mazeSizeX-1 && z != 0 && z != mazeSizeZ-1)
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

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	void CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z, int objectLayer)
	{
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.layer = objectLayer;
		myPrefab.transform.parent = parent.transform;
	}
}