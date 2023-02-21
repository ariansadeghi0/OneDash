using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Danger : MonoBehaviour
{
    #region isSafe stuff
    [SerializeField] private bool initialIsSafeValue;
    private bool _isSafe;
    public bool isSafe
    {
        get
        {
            return _isSafe;  //Getting value
        }
        set
        {
            if (value == true)
            {
                _isSafe = true;  //Setting value
                ColorGreen();   //Setting color
            }
            else if (value == false)
            {
                _isSafe = false; //Setting value
                ColorRed(); //Setting color
            }
        }
    }
    #endregion

    [Header("Grab Values")]
    [SerializeField] private bool grabEnabled;
    [SerializeField] private Color32 grabBorderColor;
    private Color32 initialBorderColor;

    [Header("Dangerblock Values")]
    [SerializeField] private SpriteRenderer borderSpriteRenderer;
    [SerializeField] private Color32 unSafeColor;
    [SerializeField] private Color32 safeColor;

    [Header("Danger Line Values")]
    [SerializeField] private SpriteShape SafeSpriteShape;
    [SerializeField] private SpriteShape UnSafeSpriteShape;

    private Vector3 initialPosition;
    private bool canGrab = false;
    private bool isBeingHeld = false;
    private float startPosX;
    private float startPosY;

    void Start()
    {
        //Getting initial position
        initialPosition = transform.position;

        //Setting initial isSafe value
        isSafe = initialIsSafeValue;
 
        if (GetComponent<SpriteRenderer>()) //If it's a danger block and not a danger line
        {
            //Setting initial border color
            initialBorderColor = borderSpriteRenderer.color;

            //Setting border to match dangerblock size
            borderSpriteRenderer.size = new Vector2(this.transform.localScale.x, this.transform.localScale.y);
            borderSpriteRenderer.gameObject.transform.localScale = new Vector3(borderSpriteRenderer.gameObject.transform.localScale.x / transform.localScale.x, borderSpriteRenderer.gameObject.transform.localScale.y / transform.localScale.y);
        }

        //Subscribing to events
        if (grabEnabled)
        {
            FindObjectOfType<PlayerScript1>().StartGame += OnGameStart;
            FindObjectOfType<PlayerScript1>().EndGame += OnGameEnd;
        }

        FindObjectOfType<PlayerScript1>().ResetGame += ResetToInitial;

        if (isSafe == true)
        {
            ColorGreen();   //Changing color to green
        }
        else if (isSafe == false)
        {
            ColorRed();   //Changing color to red
        }
    }

    void Update()
    {
        //Checking if object is being held
        if (isBeingHeld == true)
        {
            Vector3 mousePos;   //Variable to keep track of mouse position
            mousePos = Input.mousePosition; //Getting value for mouse position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Making sure object is moved from where it was grabbed
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
        }
    }

    private void OnMouseDown()
    {
        //If left clicked
        if (Input.GetMouseButtonDown(0) && canGrab == true)
        {
            Vector3 mousePos;   //Variable to keep track of mouse position
            mousePos = Input.mousePosition; //Getting value for mouse position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Getting values for start positions
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isBeingHeld = true; //Object being held by mouse

            //Changing border color
            borderSpriteRenderer.color = grabBorderColor;

            //Playing clickdown sound
            AudioManager.instance.Play("DangerClick");
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.A) && canGrab == true)    //Same as OnMouseDown
        {
            Vector3 mousePos;   //Variable to keep track of mouse position
            mousePos = Input.mousePosition; //Getting value for mouse position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Getting values for start positions
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isBeingHeld = true; //Object being held by mouse

            //Changing border color
            borderSpriteRenderer.color = grabBorderColor;

            //Playing clickdown sound
            AudioManager.instance.Play("DangerClick");
        }
        else if (Input.GetKeyUp(KeyCode.A) && canGrab == true) //Same as OnMouseUp
        {
            isBeingHeld = false;    //Object not being held by mouse

            //Changing border color
            borderSpriteRenderer.color = initialBorderColor;
        }
    }

    private void OnMouseUp()
    {
        if (canGrab == true)
        {
            isBeingHeld = false;    //Object not being held by mouse

            //Changing border color
            borderSpriteRenderer.color = initialBorderColor;
        }
    }

    private void OnGameStart()
    {
        //Letting player grab blocks
        canGrab = true;
    }

    private void OnGameEnd(bool finished, bool newHighscore)
    {
        //Not letting player grab blocks
        canGrab = false;
        isBeingHeld = false;

        //Changing border color
        borderSpriteRenderer.color = initialBorderColor;
    }

    private void ResetToInitial(float resetDuration)
    {
        if (grabEnabled)
        {
            //Setting blocks to initial positions
            LeanTween.move(this.gameObject, initialPosition, resetDuration).setEaseInOutQuart();
        }

        //Reseting isSafe
        isSafe = initialIsSafeValue;
    }

    private void ColorGreen()
    {
        if (this.GetComponent<SpriteRenderer>() == true)    //If its a danger block
        {
            this.GetComponent<SpriteRenderer>().color = safeColor; //Set color green
        }
        else if (this.GetComponent<SpriteShapeController>())    //If its a dangerline
        {
            this.GetComponent<SpriteShapeController>().spriteShape = SafeSpriteShape;
        }
    }

    private void ColorRed()
    {
        if (this.GetComponent<SpriteRenderer>() == true)    //If its a danger block
        {
            this.GetComponent<SpriteRenderer>().color = unSafeColor; //Set color red
        }
        else if (this.GetComponent<SpriteShapeController>())    //If its a dangerline
        {
            this.GetComponent<SpriteShapeController>().spriteShape = UnSafeSpriteShape;
        }
    }
}
