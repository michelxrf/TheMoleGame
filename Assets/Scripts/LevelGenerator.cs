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

	public GameObject characterController;

	public GameObject floorParent;
	public GameObject wallsParent;

	public int mazeSize;

	// 2D array representing the map
	private int[,] mapData;

	// we use these to dig through our maze and to spawn the pickup at the end
	private int mazeX = 4, mazeY = 1;

	// Use this for initialization
	void Start () {

		// initialize map 2D array
		mapData = GenerateMazeData();

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSize; z++) {
			for (int x = 0; x < mazeSize; x++) {
				if (mapData[z, x] == 1) { // solid walls
					CreateChildPrefab(wallPrefab, wallsParent, x, 1, z);

					// create floor below walls
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				} 
                else if(mapData[z, x] == 0){ // floor only
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				}
                // TODO: Generate all the other types of tiles
                // elseif(isso){bota isso};

			}
		}
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	int[,] GenerateMazeData() {
		int[,] data = new int[mazeSize, mazeSize];

		// TODO: procgen the level map
        for(int z = 0; z < mazeSize; z++)
        {
            for (int x = 0; x < mazeSize; x++)
            {
                if(x == 0 || x == mazeSize - 1 || z == 0 || z == mazeSize - 1) // map border
                {
                    data[x,z] = 1;
                }
                else
                {
                    data[x,z] = 0;
                }
            }
        }

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	void CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z) {
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
	}
}
