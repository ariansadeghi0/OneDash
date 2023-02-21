using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class ScreenshotScript : MonoBehaviour
{
    private Camera renderCam;
    int photoWidth = 1920, photoHeight = 1080;

    private void Start()
    {
        //Subscribing to event
        FindObjectOfType<PlayerScript1>().EndGame += TakeScreenshot;

        //Getting camera component
        renderCam = this.GetComponent<Camera>();
    }

    public void TakeScreenshot(bool finished, bool setNewHighscore)
    {
        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
        renderCam.targetTexture = rt;
        RenderTexture.active = rt;
        renderCam.Render();
        Texture2D screenShot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        renderCam.targetTexture = null;
        screenShot.Apply();
        GameObject image = GameObject.Find("ScreenshotImage");
        image.GetComponent<Image>().sprite = Sprite.Create(screenShot, new Rect(0, 0, photoWidth, photoHeight), new Vector2(0, 0));

        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
    }
}
