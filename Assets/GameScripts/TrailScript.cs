using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    private GameObject character;
    void Start()
    {
        //Getting character game object
        character = GameObject.FindGameObjectWithTag("Character");
    }

    void Update()
    {
        transform.position = character.transform.position;  //Updating trail position
        transform.localScale = character.transform.localScale;  //Updating trail size
    }
}
