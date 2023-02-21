using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TriggerLineScript : MonoBehaviour
{
    [Header("Referenced object")]
    [SerializeField] private Danger dangerObject;
    public Danger hiddenDangerObject;

    [Header("Colors")]
    [SerializeField] private Color32 initialColor;
    [SerializeField] private Color32 inContactColor;
    [SerializeField] private Color32 triggeredColor;

    [Header("Collider")]
    [SerializeField] private GameObject thisColliderPrefab;

    private SpriteShapeController thisSSC;
    private SpriteShapeRenderer thisSSR;

    private void Awake()
    {
        //Getting components
        thisSSC = this.GetComponent<SpriteShapeController>();
        thisSSR = this.GetComponent<SpriteShapeRenderer>();

        MakeCollider(); //Making collider
    }

    private void MakeCollider()
    {
        int verticeCount = thisSSC.spline.GetPointCount();   //Counting vertices

        if (verticeCount > 2)   //If there is more than 1 line segment
        {
            //Removing extra point until there is only two
            for (int i = verticeCount; i > 2; i--)
            {
                thisSSC.spline.RemovePointAt(i - 1);
            }
        }

        Vector3 Corner = new Vector3();
        Corner = thisSSC.spline.GetPosition(1);

        //Code for making line light up colliders
        Vector3 colliderPosition;
        float colliderRotation;
        Vector3 colliderScale;

        //Getting last spline corner's position
        Vector3 lastPosition = thisSSC.spline.GetPosition(0);

        //Getting collider position
        colliderPosition = new Vector3((Corner.x + lastPosition.x) / 2 * 0.1f, (Corner.y + lastPosition.y) / 2 * 0.1f);


        //Getting collider rotation
        colliderRotation = Mathf.Atan2(Corner.y - lastPosition.y, Corner.x - lastPosition.x) * 180 / Mathf.PI;

        //Getting collider scale
        colliderScale = new Vector3(Vector3.Distance(lastPosition, Corner), 1);

        //making collider
        var LineLightCollider = GameObject.Instantiate(thisColliderPrefab, this.transform);
        LineLightCollider.name = "Collider";
        LineLightCollider.transform.position = colliderPosition;
        LineLightCollider.transform.rotation = Quaternion.Euler(0, 0, colliderRotation);
        LineLightCollider.transform.localScale = colliderScale;
    }

    public void InContact()
    {
        thisSSR.color = inContactColor;
    }

    public void OutOfContact()
    {
        thisSSR.color = initialColor;
    }

    public void Trigger()
    {
        thisSSR.color = triggeredColor;

        dangerObject.isSafe = true;
        dangerObject.gameObject.transform.Find("HiddenLine").GetComponent<Danger>().isSafe = true;

        //Playing sound
        AudioManager.instance.Play("TriggerClick");
    }

    public void ResetToInitial()
    {
        thisSSR.color = initialColor;
    }
}
