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
    Animator m_Animator2;

    public float animSpeed = 1f;

    public PlayerMovement gObj;
    public CharacterController2D cObj;
    //public NextLevel lObj;
    public bool isJumping;

    void Start()
    {
        //lObj = GetComponent<NextLevel>();
        gObj = GameObject.Find("Player").GetComponent<PlayerMovement>();
        cObj = GameObject.Find("Player").GetComponent<CharacterController2D>();
        //Get the Animator attached to the GameObject you are intending to animate.

        m_Animator = GetComponent<Animator>();
        m_Animator2 = GetComponent<Animator>();

        //m_Animation = GetComponent<Animation>();
        m_Animator.enabled = false;
        m_Animator2.enabled = false;
    }

    void Update()
    {
        // if chest is activated
        if (gObj.advanceLevel == true)
        {
            //Send the message to the Animator to activate the trigger parameter named "Jump"
            StartCoroutine("PlayAnimation");
        }

        if (cObj.m_Grounded && isJumping) {
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

        if (cObj.m_Grounded && isJumping) {
        	Debug.Log("animating jump");
        	m_Animator2.speed = animSpeed;
        	m_Animator2.enabled = true;
        	//cObj.GetComponent<SpriteRenderer>().enabled = false;
        	m_Animator2.Play("1", 0, 0.0f);
            yield return new WaitForSeconds(0.25f);
            m_Animator2.enabled = false;
            isJumping = false;
        }
        
    }
}