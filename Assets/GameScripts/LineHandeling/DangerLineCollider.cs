using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DangerLineCollider : MonoBehaviour
{
    private SpriteShapeController hiddenLineSSC;
    private SpriteShapeRenderer hiddenLineSSR;

    void Start()
    {
        var HiddenLine = GameObject.Instantiate(this);
        HiddenLine.name = "HiddenLine";
        HiddenLine.transform.parent = this.transform;
        //Removing the script component
        Destroy(HiddenLine.GetComponent<DangerLineCollider>());
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
