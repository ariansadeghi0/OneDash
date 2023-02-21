using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LineScript : MonoBehaviour
{
    public SpriteShapeController litLine;

    [SerializeField] private GameObject lineLightColliderPrefab;
    private SpriteShapeController _SpriteShapeController;

    private SpriteShapeController hiddenLineSSC;
    private SpriteShapeRenderer hiddenLineSSR;

    void Start()
    {
        FindObjectOfType<PlayerScript1>().ResetGame += LightReset;  //Subscribing to event

        BuildHiddenLine();    //Making hidden line with the other collider

        //Getting components
        litLine = this.transform.Find("LitLine").GetComponent<SpriteShapeController>();
        _SpriteShapeController = this.GetComponent<SpriteShapeController>();

        int verticeCount = _SpriteShapeController.spline.GetPointCount();   //Counting vertices

        for (int i = 1; i < verticeCount; i++)  //Skipping first point, the start point
        {
            Vector3 Corner = new Vector3();
            Corner = _SpriteShapeController.spline.GetPosition(i);

            //Code for making line light up colliders
            Vector3 colliderPosition;
            float colliderRotation;
            Vector3 colliderScale;

            //Getting last spline corner's position
            Vector3 lastPosition = _SpriteShapeController.spline.GetPosition(i - 1);

            //Getting collider position
            colliderPosition = new Vector3((Corner.x + lastPosition.x) / 2 * 0.1f, (Corner.y + lastPosition.y) / 2 * 0.1f);
            

            //Getting collider rotation
            colliderRotation = Mathf.Atan2(Corner.y - lastPosition.y, Corner.x - lastPosition.x) * 180 / Mathf.PI;

            //Getting collider scale
            colliderScale = new Vector3(Vector3.Distance(lastPosition, Corner), 1);

            //making collider
            var LineLightCollider = GameObject.Instantiate(lineLightColliderPrefab, this.transform);
            LineLightCollider.name = "LightUpCollider";
            LineLightCollider.transform.position = colliderPosition;
            LineLightCollider.transform.rotation = Quaternion.Euler(0, 0, colliderRotation);
            LineLightCollider.transform.localScale = colliderScale;
            LineLightCollider.GetComponent<ColliderIdScript>().colliderIndex = i;
        }
    }

    public void LightUp(int index)
    {
        if (litLine.gameObject.activeInHierarchy == false)  //Activating line if it was disactivated
        {
            litLine.gameObject.SetActive(true);
        }
        if (index == 1)
        {
            litLine.spline.SetPosition(0, _SpriteShapeController.spline.GetPosition(0));
            litLine.spline.SetPosition(1, _SpriteShapeController.spline.GetPosition(1));
        }
        else if (index > 1)
        {
            try     //Will make new point if the point doesn't already exist
            {
                if (litLine.spline.GetCorner(index))
                {
                    //Point already exists
                    //Do nothing
                }
            }
            catch
            {
                litLine.spline.InsertPointAt(index, _SpriteShapeController.spline.GetPosition(index));
            }
        }
    }

    private void LightReset(float resetDuration)   //Reset line lights
    {
        //Removing points
        for (int i = litLine.spline.GetPointCount() - 1; i > 1; i--)
        {
            litLine.spline.RemovePointAt(i);
        }

        //Disactivating line
        litLine.gameObject.SetActive(false);
    }

    private void BuildHiddenLine()
    {
        var HiddenLine = GameObject.Instantiate(this);
        HiddenLine.name = "HiddenLine";
        HiddenLine.transform.parent = this.transform;
        //Removing the script component
        Destroy(HiddenLine.GetComponent<LineScript>());
        //Removing the litline child
        GameObject _litLine = HiddenLine.transform.Find("LitLine").gameObject;
        if (_litLine != null)
        {
            Destroy(_litLine);
        }
        //Getting components
        hiddenLineSSC = HiddenLine.GetComponent<SpriteShapeController>();
        hiddenLineSSR = HiddenLine.GetComponent<SpriteShapeRenderer>();
        //Setting up collider offset
        hiddenLineSSC.colliderOffset = hiddenLineSSC.colliderOffset * -1;

        StartCoroutine(DisableComponents());
    }

    IEnumerator DisableComponents()
    {
        yield return new WaitForSeconds(0.5f);

        //removing components
        Destroy(hiddenLineSSC);
        Destroy(hiddenLineSSR);
    }
}
