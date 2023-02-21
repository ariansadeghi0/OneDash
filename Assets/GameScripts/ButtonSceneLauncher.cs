using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSceneLauncher : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Awake()
    {
        ButtonScript.alreadyClicked = 0;   //Reseting value on awake
    }

    public void Clicked()
    {
        if (ButtonScript.alreadyClicked == 1)   //On first click
        {
            //Loading scene
            GameManager.Instance.LoadScene(sceneName);
        }
    }
}
