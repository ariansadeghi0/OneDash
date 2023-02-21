using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScirpt : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer GameScreen;

    private float orthoSize;

    void Start()
    {
        orthoSize = GameScreen.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Camera.main.orthographicSize = orthoSize;
    }

    void Update()
    {
        orthoSize = GameScreen.bounds.size.x * Screen.height / Screen.width * 0.5f;

        if (Camera.main.orthographicSize != orthoSize)
        {
            Camera.main.orthographicSize = orthoSize;
        }
    }
}
