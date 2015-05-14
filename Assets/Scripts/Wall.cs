using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	public Sprite dmgSprite;
	public int healthPoints = 4;

	private SpriteRenderer spriteRenderer;


	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void DamageWall (int loss){
		spriteRenderer.sprite = dmgSprite;
		healthPoints -= loss;
		if (healthPoints <= 0){
			gameObject.SetActive(false);
		}
	}
}
