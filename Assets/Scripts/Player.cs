using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Player manages player motion, damaging the walls, and the text of the 'food points' at the screen bottom.
public class Player : MovingObject {
	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f; 
	public Text foodText;

	private Animator animator;
	[SerializeField] private int food;

	protected override void Start () {
		animator = GetComponent<Animator>();
		food = GameManager.instance.playerFoodPoints; //player is destroyed and instantiated at each stage, GameManager tracks food
		UpdateFood ();
		base.Start ();
	
	}
	private void OnDisable(){ 
		GameManager.instance.playerFoodPoints = food;
	}


	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn) return;
		int horizontal = 0;
		int vertical = 0;
		horizontal = (int) Input.GetAxisRaw("Horizontal");
		vertical = (int) Input.GetAxisRaw("Vertical");
		if (horizontal != 0)
			vertical = 0;
		if (horizontal != 0 || vertical != 0)
			AttemptMove<Wall>(horizontal,vertical);

	}
	protected override void AttemptMove <T> (int xDir, int yDir){
		food--;
		UpdateFood();
		base.AttemptMove <T> (xDir, yDir);
		RaycastHit2D hit;
		CheckIfGameOver();
		GameManager.instance.playersTurn = false;
	}

	private void UpdateFood(){
		foodText.text = "Food: " + food;
	}

	private void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Exit"){
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}
		else if (other.tag == "Food"){
			food += pointsPerFood;
			other.gameObject.SetActive(false);
			foodText.text = "+" + pointsPerFood +" Food: " + food;
		}
		else if (other.tag == "Soda"){
			food += pointsPerSoda;
			other.gameObject.SetActive(false);
			foodText.text = "+" + pointsPerSoda +" Food: " + food;
		}
	}


	protected override void OnCantMove <T>(T component){
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);
		animator.SetTrigger("PlayerChop");
	}

	private void HitExit(){
		Application.LoadLevel(Application.loadedLevel);
	}
	public void LoseFood (int loss){
		animator.SetTrigger("PlayerHit");
		food -= loss;
		foodText.text = "-" + loss +" Food: " + food;
		CheckIfGameOver();
	}
	private void CheckIfGameOver(){
		if (food <= 0)
			GameManager.instance.GameOver();
	}
	private void Restart ()
	{
		//Load the last scene loaded, in this case Untitled, the only scene in the game.
		Application.LoadLevel (Application.loadedLevel);
	}
}
