﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;

	public Health hObj;
    public Score sObj;
    //public NextLevel lObj;
	SpriteRenderer sr; //
    Color srOrigColor; //

    public GameObject spawnPoint1;

	public float runSpeed = 300;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	bool gotHurt = false;
    bool isDead = false;
    bool gotHealth = false;
    public bool advanceLevel = false;

	void Start() {
        sObj = GetComponent<Score>();
		hObj = GetComponent<Health>();
        //lObj = GetComponent<NextLevel>();
		sr = GetComponent<SpriteRenderer>();
        srOrigColor = sr.color;

	}

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump")) 
        {
        	jump = true;
        	//Debug.Log("jumping");
        }

        if (Input.GetButtonDown("Crouch")) 
        {
        	crouch = true;
        	//Debug.Log("crouch down");
        } else if (Input.GetButtonUp("Crouch")) {
        	crouch = false;
        	//Debug.Log("crouch up");
        }

        if (gotHurt) {
            
            sr.color = new Color(1,1,1,0.5f);
        	
            StartCoroutine("FadeBack");
            //Debug.Log("Start Coroutine");
             // coroutine
        	//gotHurt = false;
        }

        if (gotHealth) {
            //sr.color = new Color(0,1,0);
            //sr.color = Color.white;
            sr.color = new Color (254/255f, 215/255f, 0f, 1f);
            //sr.color = new Color(1f, 0.92f, 0.016f, 1f);
            StartCoroutine("FadeBack");
            //Debug.Log("Start Coroutine");
            
        }

        // Restart level if death conditions are met
        if (isDead) {
            isDead = false;
            Application.LoadLevel(Application.loadedLevel);

        }

    }

    IEnumerator FadeBack() {
        if (gotHealth) {
            yield return new WaitForSeconds(0.5f);
            sr.color = srOrigColor;
            gotHealth = false;
        }

        if (gotHurt) {
            yield return new WaitForSeconds(0.5f);
            sr.color = srOrigColor;
            gotHurt = false;
        }
        //sr.color = new Color(0,1,0);
        
        //Debug.Log("Finish Coroutine");
    }

    void FixedUpdate () 
    {
    	// Move our character
    	controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
    	jump = false;

        // Respawn to beginning of level when health is 0
        if (hObj.health == 0) {

            isDead = true;

            // for CHECKPOINTS (possibly)
            //this.transform.position = spawnPoint1.transform.position;
            //hObj.health = 5; // reset health
            //reset level

        }
    	
    }

    // Player triggers with colliders

    void OnTriggerEnter2D(Collider2D col) {

    	if(col.gameObject.tag == "Coins") {
			Debug.Log("got coin");

            // Add points to player score
            sObj.AddPoints(5);
			Destroy(col.gameObject);
		} 


		// When player gets hurt

		else if (col.gameObject.tag == "Enemy") {

			// Health decrease by 1
			hObj.TakeDamage(1);

			// Change player's color
			gotHurt = true;


			//Debug.Log("got spike hurt");
		}

        // When player gets health

        else if (col.gameObject.tag == "Health") {

            // Health decrease by 1
            hObj.AddHealth();
            //Destroy(col.gameObject);

            gotHealth = true;

            //Debug.Log("got healthpack");
        }

        // When player dies
        else if (col.gameObject.tag == "DeathZone") {
            // Respawn player to beg of level

            //Debug.Log("player has died");
            isDead = true;

            //this.transform.position = spawnPoint1.transform.position;
        }

        // When player reaches end of level
        else if (col.gameObject.tag == "Finish") {
            advanceLevel = true; 
            PlayerPrefs.SetInt("Player Score", sObj.score);
            PlayerPrefs.SetInt("Player Health", hObj.health);
            //Debug.Log("End of level");
            
        }
    }

    // Player collides with other colliders
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            // Health decrease by 1
            hObj.TakeDamage(1);

            // Change player's color
            gotHurt = true;


            //Debug.Log("got HIT");
        }
    }


}
