using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove; //enemy only moves every other turn
	private bool stuck; //trying to guess if the enemy is stuck on an obstacle
	private Vector3 lastPos;


	protected override void Start () {
		GameManager.instance.AddEnemyToList(this);
		animator = GetComponent<Animator>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		animator.speed = 1 + Random.Range (-.1f,.1f);
		base.Start ();
	}

	protected override void AttemptMove<T> (int xDir, int yDir){
		if (skipMove){
			skipMove = false;
			return;
		}
		base.AttemptMove <T> (xDir, yDir);
		
		lastPos = transform.position;
		skipMove = true;
	}


	public void MoveEnemy(){
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x)<float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1:-1;
		else
			xDir = target.position.x > transform.position.x ? 1:-1;
		/*
		 * Fun idea to have the zombie move at random if he was stuck. Didn't really work.
		if ((transform.position.y - lastPos.y) < float.Epsilon && (transform.position.x - lastPos.x) < float.Epsilon){

			if (Random.value >= 0.5)
			{
				xDir = xDir*-1;
			} else{
			yDir = yDir * -1;
			}
		}
		*/

		AttemptMove <Player> (xDir, yDir);

	}

	public void KillYourself(){
		skipMove = true;
		Destroy (gameObject, 2f);
	}

	protected override void OnCantMove <T> (T component){
		Player hitPlayer = component as Player;
		animator.SetTrigger("enemyAttack");
		hitPlayer.LoseFood (playerDamage);
	
	}
}
