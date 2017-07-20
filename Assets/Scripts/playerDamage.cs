﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDamage : MonoBehaviour {

	public int maxHealth = 4;
	public Sprite[] damageSprites;
	public SpriteRenderer damageRend;
	public SpriteRenderer shieldRend;

	public int currentHealth;
	public float invincibilityTime=0.5f;
	private float timeToNextColl = 0.0f;
	private bool isShielded = false;

	public float shieldRechargeTime=8.0f;
	public float shieldDuration=2.0f;
	private float timeToNextShield=0.0f;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		shieldRend.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)&&Time.time>timeToNextShield) {
			StartCoroutine ("shieldOn");
			//shieldOn ();
			timeToNextShield=Time.time+shieldRechargeTime;
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag("meteor")){
			ItemManager.instance.itemList.Remove (coll.gameObject);
			Destroy (coll.gameObject);
		}
		if (coll.gameObject.CompareTag("pickUp")){
			currentHealth = maxHealth;
			damageRend.sprite = null;
			Destroy (coll.gameObject);
			playerScore.instance.addScore (50);
		}
		if (Time.time>timeToNextColl && !coll.gameObject.CompareTag("pickUp")) {
			if (!isShielded) {
				LoseHealth ();
				timeToNextColl = Time.time + invincibilityTime;
			}
		}
	}

	void LoseHealth(){
		//Debug.Log (currentHealth);
		currentHealth--;
		//Debug.Log (currentHealth);
		if (currentHealth <= 0) {
			Destroy (gameObject);
		}
		if (0 < currentHealth && currentHealth < 4) {
			damageRend.sprite = damageSprites [3 - currentHealth];
		}
	}

	IEnumerator shieldOn(){
		isShielded = true;
		shieldRend.enabled = true;
		yield return new WaitForSeconds(shieldDuration);

		isShielded = false;
		shieldRend.enabled = false;
	}

}