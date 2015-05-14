using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;



//BoardManager lays out background tiles and places relevant game pieces.
public class BoardManager : MonoBehaviour {
	public class Count {
		public int maximum;
		public int minimum;

		public Count (int min, int max){
			minimum = min;
			maximum = max;
		}

	
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(4,8);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>(); //storing valid grid positions

	void InitializeList(){
		gridPositions.Clear();
		for (int x = 1; x < columns-1; x++)		{
			for (int y = 1; y < rows-1; y++){
				gridPositions.Add (new Vector3(x,y,0f));
			}
		}
	}
	 	
	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns + 1;x++){
			for (int y = -1; y < rows + 1; y++){
				GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
				if( x==-1 || x == columns || y==-1 || y==rows){
					toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)];
				}
				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f),Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition(){
		int randomIndex = Random.Range(0,gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex); //once a location in the gridPositions list is used, it's dropped from the list
		return randomPosition;
	}
	//TODO this will very likely break if there are more objects in ObjectCount than in gridPositions.
	void LayoutObjectsAtRandom (GameObject[] tileArray, int min, int max){
		int ObjectCount = Random.Range(min, max+1);
		for(int i = 0; i < ObjectCount; i++)
		{
			Vector3 randomPosition = RandomPosition();
			GameObject toInstantiate = tileArray[Random.Range(0,tileArray.Length)];
			GameObject instance = Instantiate(toInstantiate, randomPosition, Quaternion.identity) as GameObject;


		}
	}
	public void SetupScene(int level){
		BoardSetup();
		InitializeList();
		//on smaller boards the exit doesn't spawn if it's at the end of this function, presumably the TODO above is related.
		Instantiate (exit, new Vector3(columns - 1, rows - 1, 0f),  Quaternion.identity); //exit's always at the top right. no need to check this space since the borders are always clear
		int enemyCount = (int)Mathf.Log(level, 2f); // logarithmic increase in enemies

		LayoutObjectsAtRandom(enemyTiles, enemyCount, enemyCount); //fixed enemies per level

		LayoutObjectsAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

		LayoutObjectsAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
		LayoutObjectsAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

	}


}
