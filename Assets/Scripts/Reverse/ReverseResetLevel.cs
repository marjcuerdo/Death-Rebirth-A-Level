using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReverseResetLevel : MonoBehaviour
{

	public ReverseTimer tObj;
	public bool restartedLevel = false;

    public void Start() {
        tObj = GameObject.Find("Player").GetComponent<ReverseTimer>();
    }
    
    public void RestartLevel() {
    	// continue timer 
        PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
        PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
        
    	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}