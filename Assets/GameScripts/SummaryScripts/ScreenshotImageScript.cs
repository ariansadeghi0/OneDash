using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotImageScript : MonoBehaviour
{
    private bool alreadyClicked = false;

    private Animator thisAnimator;

    void Start()
    {
        thisAnimator = GetComponent<Animator>();    //Getting component
    }
    public void OnEnter()
    {
        //Calling animation
        thisAnimator.SetBool("Hovering", true);

        AudioManager.instance.Play("ButtonHover");  //Playing sound
    }

    public void OnExit()
    {
        //Calling animation
        thisAnimator.SetBool("Hovering", false);
        thisAnimator.SetBool("Click", false);

        alreadyClicked = false;
    }

    public void OnClick()
    {
        if (alreadyClicked == false)
        {
            //Calling animation
            thisAnimator.SetBool("Click", true);

            AudioManager.instance.Play("ScreenshotClick");  //Playing sound

            alreadyClicked = true;
        }
    }
}
