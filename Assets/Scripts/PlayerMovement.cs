using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;

	public Health hObj;
    public Score sObj;
    public Timer tObj;
    public Wind wObj;
    public ResetLevel rObj;

	SpriteRenderer sr; //
    Color srOrigColor; //

    public GameObject spawnPoint1;

	public float runSpeed = 300;

	float horizontalMove = 0f;
	bool jump = false;
	//bool crouch = false;
	bool gotHurt = false;
    bool isDead = false;
    bool gotHealth = false;
    public bool advanceLevel = false;
    public bool isNewGame = true;

    public int deathCounter = 0;
    public TextMeshProUGUI deathText;

    SpriteRenderer[] sprites;

    public NextLevel lObj;

    public AudioSource coinAudio;
    public AudioSource deathAudio;

    public Animator anim;

    public CharacterController2D cObj;

    public int levelReached = 1;
    

	void Awake() {
        deathCounter = PlayerPrefs.GetInt("Player Deaths"); ////////////////////
    }

    void Start() {
        cObj = GetComponent<CharacterController2D>();
        rObj = GameObject.Find("God").GetComponent<ResetLevel>();
        anim = GetComponent<Animator>();
        sObj = GetComponent<Score>();
		hObj = GetComponent<Health>();
        tObj = GetComponent<Timer>();
        if (SceneManager.GetActiveScene().name == "Level1") {
            PlayerPrefs.SetInt("Level Reached", 1);
        } else if (SceneManager.GetActiveScene().name == "Level2") {
            PlayerPrefs.SetInt("Level Reached", 2);
        } else if (SceneManager.GetActiveScene().name == "Level3") {
            PlayerPrefs.SetInt("Level Reached", 3);
        } else if (SceneManager.GetActiveScene().name == "Level4") {
            PlayerPrefs.SetInt("Level Reached", 4);
        } else if (SceneManager.GetActiveScene().name == "Level5") {
            PlayerPrefs.SetInt("Level Reached", 5);
        }
        if (SceneManager.GetActiveScene().name == "Level5") {
            wObj = GetComponent<Wind>();
        }
        lObj = GameObject.Find("Chest").GetComponent<NextLevel>();

		sr = GetComponent<SpriteRenderer>();

        srOrigColor = sr.color;

        sprites = GetComponentsInChildren<SpriteRenderer>();

        /*for (int i=0; i < spriteRenderers.Count; ++i) {
            colors.Add(spriteRenderers.material.color);
            renderer.material.color = new Color(1,1,1,0.5f);
        }*/
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

        if (horizontalMove == 0 && cObj.m_Grounded) {
            anim.SetBool("isRunning", false);
        } else if (horizontalMove > 0 && !cObj.m_Grounded){
            anim.SetBool("isRunning", false);
        } else {
            anim.SetBool("isRunning", true);
        }

         deathText.text = deathCounter.ToString(); // display current # deaths


        // for crouching
        /*if (Input.GetButtonDown("Crouch")) 
        {
        	crouch = true;
        	//Debug.Log("crouch down");
        } else if (Input.GetButtonUp("Crouch")) {
        	crouch = false;
        	//Debug.Log("crouch up");
        }*/

        if (gotHurt) {

            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color(1,1,1,0.5f);
            }

            StartCoroutine("FadeBack");
        }

        if (gotHealth) {
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color (254/255f, 215/255f, 0f, 1f);
            }
            StartCoroutine("FadeBack");         
        }

        // Restart level if death conditions are met
        if (isDead) {

            deathCounter += 1; // increment death
           // Debug.Log(deathCounter);
            PlayerPrefs.SetInt("Player Deaths", deathCounter); ///////////////////

            isDead = false;

            // reload current level / beginning of checkpoint
            rObj.RestartLevel();
        }

        // when chest/finish point is reached, load next level
        if (advanceLevel) {
            lObj.LoadNextScene();
        }

        // if wind is blowing on Level 5
        if (SceneManager.GetActiveScene().name == "Level5") {
            if (wObj.windIsBlowing) {
                for (int i=0; i <sprites.Length; i++) {
                    sprites[i].color = new Color (0f, 0f, 255f/255f, 1f);
                }

                StartCoroutine("FadeBack");   
            }
        }

    }

    IEnumerator FadeBack() {
        if (gotHealth) {
            yield return new WaitForSeconds(0.5f);
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = srOrigColor;
            }
            gotHealth = false;
        }

        if (gotHurt) {
            yield return new WaitForSeconds(0.5f);
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = srOrigColor;
            }
            //sr.color = srOrigColor;
            gotHurt = false;
        }

        if (SceneManager.GetActiveScene().name == "Level5") {
            if (wObj.windIsBlowing) {

                yield return new WaitForSeconds(3f);
                for (int i=0; i <sprites.Length; i++) {
                    sprites[i].color = srOrigColor;
                }
                //sr.color = srOrigColor;
                //gotHurt = false;
            }
        }
    }

    void FixedUpdate () 
    {
    	// Move our character
    	controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

        // Respawn to beginning of level when health is 0
        if (hObj.health == 0) {

             // continue timer when player's health runs out
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            
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
			//Debug.Log("got coin");

            // Add points to player score
            sObj.AddPoints(10);
            coinAudio.Play();
			col.gameObject.SetActive(false);
		} 


		// When player gets hurt
		else if (col.gameObject.tag == "Enemy") {

			// Health decrease by 1
			hObj.TakeDamage(1);
            //Debug.Log("HEALTH: " + hObj.health);
            hObj.tookDamage = true; /////

			// Change player's color
			gotHurt = true;


			//Debug.Log("got spike hurt");
		}

        // When player gets health pack
        else if (col.gameObject.tag == "Health") {


            // Health decrease by 1
            hObj.AddHealth();
            //Debug.Log("HEALTH: " + hObj.health);

            // make health pack inactive
            col.gameObject.SetActive(false);

            gotHealth = true;
        }

        // When player dies
        else if (col.gameObject.tag == "DeathZone") {
            // Respawn player to beg of level

            // continue timer when player dies
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            deathAudio.Play();
            isDead = true;
        }

        // When player reaches end of level
        else if (col.gameObject.tag == "Finish") {

            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining);
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            PlayerPrefs.SetInt("Player Score", sObj.score);
            PlayerPrefs.SetInt("Player Health", hObj.health);
            PlayerPrefs.SetInt("Player Deaths", deathCounter); ///////////////////////
            PlayerPrefs.SetInt("Extra Hearts", hObj.currentExtraHearts);
            PlayerPrefs.SetInt("Took Damage", (hObj.tookDamage ? 1 : 0));

            if (SceneManager.GetActiveScene().name == "WinScreen") {
                PlayerPrefs.SetFloat("TimeRem", 900);
                PlayerPrefs.SetFloat("TimeInc", 0);
            }

            isNewGame = false;
            advanceLevel = true;
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

            // reset deaths when exit game
    public void OnApplicationQuit(){
         PlayerPrefs.SetInt("Player Deaths", 0);
    }

}
