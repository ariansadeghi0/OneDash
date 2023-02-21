using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonScript))]
public class SummaryBtnsScript : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [Header("Border values")]
    [SerializeField] private Image borderImage;
    [SerializeField] private Color32 onClickBorderColor;

    private void Awake()
    {
        ButtonScript.alreadyClicked = 0;   //Reseting value on awake
    }
    public void Clicked()
    {
        if (ButtonScript.alreadyClicked == 1)   //On first click
        {
            borderImage.color = onClickBorderColor;   //Changing border color

            //Loading scene
            GameManager.Instance.LoadScene(sceneName);
        }
    }
}
