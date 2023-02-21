using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummaryBackgroundScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public event Action ResetCall;

    [Header("Border Values")]
    [SerializeField] private Image Border;
    [SerializeField] private Color32 HoveredBorderColor;
    [SerializeField] private Color32 UnHoveredBorderColor;
    [SerializeField] private Color32 ClickedBorderColor;

    private bool canBeClicked = false;
    private bool isOnBackground = false;

    void Start()
    {
        //Subscribing to events
        FindObjectOfType<PlayerScript1>().EndGame += OnGameEnd;
        FindObjectOfType<PlayerScript1>().ResetGame += OnReset;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canBeClicked == true && isOnBackground == true)
        {
            //Calling game reset
            ResetCall?.Invoke();
            //Playing summary out sound
            AudioManager.instance.Play("SummaryOut");
            //Changing color of border
            Border.color = new Color32(ClickedBorderColor.r, ClickedBorderColor.g, ClickedBorderColor.b, ClickedBorderColor.a);
        }
    }

    private void OnGameEnd(bool finished, bool setNewHighscore)
    {
        canBeClicked = true;    //Can be clicked

        //Changing color of border
        Border.color = new Color32(UnHoveredBorderColor.r, UnHoveredBorderColor.g, UnHoveredBorderColor.b, UnHoveredBorderColor.a);
    }

    private void OnReset(float resetDuration)
    {
        canBeClicked = false;   //Can't be clicked
        isOnBackground = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canBeClicked == true)       //If it can be clicked on
        {
            //Calling game reset
            ResetCall?.Invoke();
            //Playing summary out sound
            AudioManager.instance.Play("SummaryOut");
            //Changing color of border
            Border.color = new Color32(ClickedBorderColor.r, ClickedBorderColor.g, ClickedBorderColor.b, ClickedBorderColor.a);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnBackground = true;

        if(canBeClicked == true)
        {
            //Changing color of border
            Border.color = new Color32(HoveredBorderColor.r, HoveredBorderColor.g, HoveredBorderColor.b, HoveredBorderColor.a);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnBackground = false;

        if (canBeClicked == true)
        {
            //Changing color of border
            Border.color = new Color32(UnHoveredBorderColor.r, UnHoveredBorderColor.g, UnHoveredBorderColor.b, UnHoveredBorderColor.a);
        }
    }
}
