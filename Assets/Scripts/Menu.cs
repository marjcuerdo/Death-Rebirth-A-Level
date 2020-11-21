using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	bool menuShown = false;

	void Awake() {
		foreach (Transform child in this.transform)
         {
             
                 child.gameObject.SetActive(false);
         }
	}

	void Update() {
		foreach (Transform child in this.transform)
         {
         	if (menuShown == false) {
         		if (Input.GetKeyDown(KeyCode.Tab))
             {
                 child.gameObject.SetActive(true);
                 menuShown = true;
             }
            } else if (menuShown) {
				if (Input.GetKeyDown(KeyCode.Tab))
             	{
                 child.gameObject.SetActive(false);
                 menuShown = false;
             	}
            }


         }
	}
}
