using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

	Scene m_Scene;
	string sceneName;
    
    private void Start() {
    	m_Scene = SceneManager.GetActiveScene();
    }

    public void LoadNextScene() {

        if (m_Scene.name == "StartScreen") {
            SceneManager.LoadScene("Level1");
        } else if (m_Scene.name == "Level1") {
            Debug.Log("Loading Level2");
    		SceneManager.LoadScene("Level2");
    	} else if (m_Scene.name == "Level2") {
            SceneManager.LoadScene("Level3");
        } else if (m_Scene.name == "Level3") {
            SceneManager.LoadScene("Level4");
        } else if (m_Scene.name == "Level4") {
            SceneManager.LoadScene("Level5");
        } else if (m_Scene.name == "Level5") {
            SceneManager.LoadScene("WinScreen");
        } else if (m_Scene.name == "WinScreen") {
            SceneManager.LoadScene("Level1");
        } else
        { 
            Debug.Log("Loading nothing :(");
        
        }
    }
}
