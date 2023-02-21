using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField] private bool spinEnabled;
    [SerializeField] private bool clockwiseSpin;
    [SerializeField] private float speed;

    void Update()
    {
        if (spinEnabled == true)
        {
            if (clockwiseSpin == true)  //Rotating clockwise
            {
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
            }
            else if (clockwiseSpin == false)    //Rotating counter-clockwise
            {
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }
}
