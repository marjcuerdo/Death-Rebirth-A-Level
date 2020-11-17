using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 5;
    public int numOfHearts = 5;
    public int currentExtraHearts = 0;

    //public Image[] hearts;
    public List<Image> hearts;

    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite extraHeart;

    //public Image extraHeartImg;

    public PlayerMovement gObj;

    public int j = 0;
    public int k = 0;
    public bool tookDamage = false;

    public AudioSource powerUpAudio;
    public AudioSource damageAudio;

    void Awake() {

        gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();

        if (gObj.isNewGame == false) {
            //health = PlayerPrefs.GetInt("Player Health");
            currentExtraHearts = PlayerPrefs.GetInt("Extra Hearts");
            health = PlayerPrefs.GetInt("Player Health");
            //Debug.Log("getting health: " + health);
            //Debug.Log("getting extra");
            //j = PlayerPrefs.GetInt("JCounter");
            //k = PlayerPrefs.GetInt("KCounter");
            j = 0;
            k = 0;
            tookDamage = (PlayerPrefs.GetInt("Took Damage") != 0);
            //gObj.isNewGame = true;
        }


    }

    void Update() {

        if (health > numOfHearts) {
            currentExtraHearts = health - numOfHearts; 
           // Debug.Log("currentExtraHearts: " + currentExtraHearts);

        } else {
            currentExtraHearts = 0;
        }

    	for (int i=0; i < hearts.Count; i++) {
            //Debug.Log("currentExtraHearts: " + currentExtraHearts);
    		
            if ( (currentExtraHearts > 0)  && (i >= 4) ) { // greater than 5 hearts

                if (!tookDamage) {
                    while ( (j < currentExtraHearts) && ( ((i+j+1) >= 0) && ( (i+j+1) < hearts.Count ) ) ) {
                        hearts[i+j+1].sprite = extraHeart;

                        hearts[i+j+1].enabled = true; // running wrong
                        j+=1;
                    }
                    // nothing happens to hearts afterwards
                    if ( ((i+j+1) >= 0) && ( (i+j+1) < hearts.Count) ) {
                    	hearts[i+j+1].enabled = false; // turn off?
                    }

                } else {
                    Debug.Log("calling damage");
                    while ( (k < currentExtraHearts) && ( ((i+k+2) >= 0) && ( (i+k+2) < hearts.Count) )) {
                        hearts[i+k+2].enabled = false;
                        k+=1;
                    }
                }

                
            } //else if (i < health)  {//&& (health < maxHearts) {
            else if ( i < health )  {
                hearts[i].sprite = fullHeart; // use full heart image 

            } else {
                // when damage is being done

                if (i <= 4) {
                    //Debug.Log("LOOP 1");
    			     hearts[i].sprite = emptyHeart; // use empty heart when health decreased
                } else if (i > 4) { // selects extra hearts and turns them off
                    hearts[i].enabled = false;
                }
    		} 
    	}
        j = 0;
        k = 0;
        tookDamage = false;

    }

    // decrease health when taking damange
    public void TakeDamage(int damage) {
    	health -= damage;
        damageAudio.Play();
        //
        Debug.Log("HEALTH: " + health);
    }

    // add health with health pack or with points
    public void AddHealth() {
        if (health < 8) { // 8 is max health with bonus
            health += 1;
            powerUpAudio.Play();
        }

    }

    // reset health on game exit
    public void OnApplicationQuit(){
         PlayerPrefs.SetInt("Player Health", 5);
         PlayerPrefs.SetInt("Extra Hearts", 0);
         PlayerPrefs.SetInt("Took Damage", (tookDamage ? 1 : 0));
         //Debug.Log("Reset health");
    }

}
