using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// GameManager tracks score and current level between scene loads. 
public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = .05f;
	public static GameManager instance = null; //holder to check that we're a singleton
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private int level = 311;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private BoardManager boardScript;
	private bool doingSetup; //prevents the player from moving





	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject); //singletoooooon
		DontDestroyOnLoad(gameObject);
		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager>();
		InitLevel();
	}

	
	void OnLevelWasLoaded(){
		level++;
		InitLevel();
	}
	//this method was called 'InitGame' at first but that seemed danged confusing. Makes a new level.
	void InitLevel(){
		doingSetup = true;

		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();

		levelText.text = "Day " + level;
		levelImage.SetActive(true);
		enemies.Clear();
		boardScript.SetupScene(level);
		Invoke("HideLevelImage", levelStartDelay);
	}

	private void HideLevelImage(){
		levelImage.SetActive(false);
		doingSetup = false;
	}


	public void GameOver(){
		levelText.text = "GAME OVER\n\nAfter " +level +" days, you starved.";
		levelImage.SetActive(true);
		enabled = false;
	}

	void Update () {
		if (playersTurn || enemiesMoving || doingSetup)
			return;
		StartCoroutine(MoveEnemies());
	
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

//not currently triggered, this functionality lets a later powerup clear the enemies on the board
public void Bomba(){
		playersTurn = true;
		Debug.Log ("Destroying all enemeis");
		for (int i = 0; i < enemies.Count; i++){
			enemies[i].KillYourself();
		}
		enemies.Clear();

	}

	IEnumerator MoveEnemies(){
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0){
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++){
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	
	}
}
