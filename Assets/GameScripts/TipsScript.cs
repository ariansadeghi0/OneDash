using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TipsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea] [SerializeField] private string tipText;
    [SerializeField] private TextMeshProUGUI tipTextBox;

    [SerializeField] [Range(1.01f, 1.5f)] private float scaleFactor;
    [SerializeField] private float scaleTime = 0;
    private Vector3 initialScale;
    private Vector3 newScale;

    private TextMeshProUGUI thisText;

    private void Awake()
    {
        //Getting scales
        initialScale = transform.localScale;
        newScale = new Vector3(initialScale.x * scaleFactor, initialScale.y * scaleFactor);

        //Getting text component
        thisText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (thisText.color.a != 0)  //If color is not transparent
        {
            //Play soundeffect
            AudioManager.instance.Play("ButtonHover");
            //Scale up
            LeanTween.scale(gameObject, newScale, scaleTime);
            //Displaying tip
            tipTextBox.text = tipText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (thisText.color.a != 0)  //If color is not transparent
        {
            //Scale down
            LeanTween.scale(gameObject, initialScale, scaleTime);
            //Removing tip from display
            tipTextBox.text = "";
        }
    }
}
