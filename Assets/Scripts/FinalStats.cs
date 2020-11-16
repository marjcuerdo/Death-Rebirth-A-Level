using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinalStats : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
   	public TextMeshProUGUI timeText;

    public SpriteRenderer medal;

    public Sprite goldMedal;
    public Sprite silverMedal;
    public Sprite bronzeMedal;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("Player Score").ToString();
        //timeText.text = PlayerPrefs.GetInt("TimeInc").ToString();
        Debug.Log("TIME: " + PlayerPrefs.GetFloat("Player Score").ToString());
        DisplayTime(PlayerPrefs.GetFloat("TimeInc"));
        medal = GetComponent<SpriteRenderer>();

    }

    // display text in UTC format
	void DisplayTime(float timeToDisplay)
    {
    	timeToDisplay += 1;

    	//float fTimeToDisplay = (float) timeToDisplay;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeToDisplay <= 480) {
            medal.sprite = goldMedal;
        } else if (timeToDisplay <= 660) {
            medal.sprite = silverMedal;
        } else {
            medal.sprite = bronzeMedal;
        }
    }

    // reset timer when exit game
    public void OnApplicationQuit(){
         PlayerPrefs.SetFloat("TimeRem", 300);
         PlayerPrefs.SetFloat("TimeInc", 0);
         PlayerPrefs.SetInt("Player Score", 0);
         PlayerPrefs.SetInt("Player Health", 5);
         //Debug.Log("Reset score");
    }
}
