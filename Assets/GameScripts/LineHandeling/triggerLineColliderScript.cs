using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class triggerLineColliderScript : MonoBehaviour
{
    private TriggerLineScript parentScript;

    private bool inContact;
    private bool triggered;

    private void Awake()
    {
        //Subscribing to event
        FindObjectOfType<PlayerScript1>().ResetGame += OnReset;

        //Getting parent script
        parentScript = transform.parent.gameObject.GetComponent<TriggerLineScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character") && GameManager.Instance.GameActive && GameManager.Instance.CanGrab)
        {
            inContact = true;   //Is in contact

            parentScript.InContact();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Character") && GameManager.Instance.GameActive && GameManager.Instance.CanGrab && triggered == false)
        {
            inContact = false;   //Is not in contact

            parentScript.OutOfContact();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inContact == true && triggered == false)
        {
            triggered = true;  //Is triggered

            parentScript.Trigger();
        }
    }

    private void OnReset(float resetDuration)
    {
        //Reseting values
        inContact = false;
        triggered = false;

        parentScript.ResetToInitial();
    }
}
