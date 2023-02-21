using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [HideInInspector] public static int alreadyClicked = 0; //Clicked counter

    private Animator thisAnimator;

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
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
    }

    public void OnClick()
    {
        if (alreadyClicked == 0)
        {
            //Calling animation
            thisAnimator.SetBool("Click", true);

            AudioManager.instance.Play("ButtonClick");  //Playing sound
        }
        alreadyClicked++;  //Is Clicked
    }
}
