//Attach this script to a GameObject with an Animator component attached.
//For this example, create parameters in the Animator and name them “Crouch” and “Jump”
//Apply these parameters to your transitions between states

//This script allows you to trigger an Animator parameter and reset the other that could possibly still be active. Press the up and down arrow keys to do this.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimateObject : MonoBehaviour
{
    Animator m_Animator;
    Animation m_Animation;

    public float animSpeed = 1f;

    public PlayerMovement gObj;
    //public NextLevel lObj;

    void Start()
    {
        //lObj = GetComponent<NextLevel>();
        gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();

        //Get the Animator attached to the GameObject you are intending to animate.

        m_Animator = GetComponent<Animator>();

        //m_Animation = GetComponent<Animation>();
        m_Animator.enabled = false;
    }

    void Update()
    {
        // if chest is activated
        if (gObj.advanceLevel == true)
        {
            //Send the message to the Animator to activate the trigger parameter named "Jump"
            StartCoroutine("PlayAnimation");
        }
    }

    IEnumerator PlayAnimation() {
        // if chest is activated
        if (gObj.advanceLevel == true) {
            m_Animator.speed = animSpeed;
            m_Animator.enabled = true;
            yield return new WaitForSeconds(0.65f);
            //Debug.Log("running coroutine");
            m_Animator.enabled = false;
            //Debug.Log("stopped animation");/*
            gObj.isNewGame = false;
            Debug.Log("not a new game");
            gObj.advanceLevel = false;
            Debug.Log("next level"); 
            //Debug.Log("reset animation");
            //lObj.LoadNextScene();
        }
        
    }
}