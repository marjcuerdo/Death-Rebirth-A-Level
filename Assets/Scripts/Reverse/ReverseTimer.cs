using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReverseTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float timeRemaining = 900; 
    public float timeInc = 0;
    public bool timerIsRunning = false;

    public ReversePlayerMovement gObj; 
    public ReverseScore sObj;
    public ReverseHealth hObj;

    void Awake() {
        gObj = GameObject.Find("Player").GetComponent<ReversePlayerMovement>(); // instantiate to access Player
        sObj = GameObject.Find("Player").GetComponent<ReverseScore>();
        hObj = GameObject.Find("Player").GetComponent<ReverseHealth>();
        //if (gObj.isNewGame == false) {
            //Debug.Log("getting time: " + PlayerPrefs.GetFloat("TimeInc").ToString());
            // get current time
            timeInc = PlayerPrefs.GetFloat("TimeInc");
            timeRemaining = PlayerPrefs.GetFloat("TimeRem");
            //Debug.Log("time again: " + PlayerPrefs.GetFloat("TimeInc").ToString());
        //}
    }

    void Start() {
        timerIsRunning = true;
    }

    void Update()
    {

        /*if (gObj.isNewGame == false) {
            timeInc = PlayerPrefs.GetFloat("TimeInc");
            timeRemaining = PlayerPrefs.GetFloat("TimeRem");
        }*/

        // while timer is running
        if (timerIsRunning)
        {
            // when 10 mins or 600 secs aren't up yet
            //if (timeRemaining > 1 )
            //{
                timeRemaining -= Time.deltaTime; // decrement from 5 mins/300 secs
                timeInc += Time.deltaTime; // increment to current playing time
                DisplayTime("Current playing time: ", timeInc); // continuously update time
            //}
            /*else
            {
                //Debug.Log("Time has been met!");
                timeInc += Time.deltaTime; // continue incrementing after 5 mins
                timeText.color = new Color32( 0 , 254 , 111, 255 ); // change text to lime green
                DisplayTime("Required play time met! ", timeInc); // notify user time's up
                timeRemaining = 0;
                //timerIsRunning = false; // use to stop timer when time is met
            }*/
            if (timeRemaining <= 0) {
                PlayerPrefs.SetFloat("TimeRem", timeRemaining);
                PlayerPrefs.SetFloat("TimeInc", timeInc);
                PlayerPrefs.SetInt("Player Score", sObj.score);
                PlayerPrefs.SetInt("Player Health", hObj.health);
                PlayerPrefs.SetInt("Player Deaths", gObj.deathCounter);
                SceneManager.LoadScene("TimeUpScreen");
            }
        }
    }

    // display text in UTC format
    void DisplayTime(string txt, float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format(txt + "{0:00}:{1:00}", minutes, seconds);
    }

    // reset timer when exit game
    public void OnApplicationQuit(){
         PlayerPrefs.SetFloat("TimeRem", 900);
         PlayerPrefs.SetFloat("TimeInc", 0);
         //Debug.Log("Reset score");
    }


}
