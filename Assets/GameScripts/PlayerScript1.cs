using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript1 : MonoBehaviour
{
    public event Action StartGame;    //Game has started
    public event Action<bool, bool> EndGame;  //Game has ended  //bools for whether game finished or not and if new Highscore is achieved
    public event Action<float> ResetGame;  //Reset things to their initial values //Float for reset duration
    public event Action<float, float, float, float> SetTimer;    //For giving the initial score value to the score timer

    [Header("Initial Values")]
    [SerializeField] private Vector3 initialCharacterSize;
    [SerializeField] private Vector3 initialCharacterPosition;
    [SerializeField] private float initialScore;

    [Header("Character Resizing")]
    [SerializeField] private bool resizable;
    [SerializeField][Range(0.3f, 1.0f)] private float resizeFactor;
    [SerializeField][Range(0.1f, 1.0f)] private float minCharacterSize;
    [SerializeField][Range(1.0f, 2.0f)] private float maxCharacterSize;

    [Header("Required Values for Stars")]
    [SerializeField] private float OneStarValue;
    [SerializeField] private float TwoStarValue;
    [SerializeField] private float ThreeStarValue;

    [Header("Character Border Values")]
    [SerializeField][Range(0, 255)] private byte MouseDownBorderAlpha;
    [SerializeField] [Range(0, 255)] private byte MouseUpBorderAlpha;

    private SpriteRenderer characterBorder;

    private bool isBeingHeld = false;
    private float startPosX;
    private float startPosY;
    private int collisionCount;
    private bool shrinkKeyPressed;
    private bool growKeyPressed;
    private bool firstReset = true;
    private bool alreadyDone = false;
    private float resetDuration = 1f;

    private Vector3 _sizeScale;
    private Vector3 sizeScale {
        get{
            return _sizeScale;
        }
        set{
            if (value.x <= minCharacterSize || value.y <= minCharacterSize)
            {
                _sizeScale = new Vector3(minCharacterSize, minCharacterSize);
            }
            else if (value.x >= maxCharacterSize || value.y >= maxCharacterSize)
            {
                _sizeScale = new Vector3(maxCharacterSize, maxCharacterSize);
            }
            else
            {
                _sizeScale = value;
            }
        }
    }

    void Start()
    {
        //Loading skins
        LoadSkins();

        //Giving star values to game manager
        GameManager.Instance.SetStarValues(OneStarValue, TwoStarValue, ThreeStarValue);

        //Subscribing to events
        ResetGame += OnReset;
        EndGame += OnGameEnd;
        FindObjectOfType<SummaryBackgroundScript>().ResetCall += OnResetCall;
        FindObjectOfType<TimerScript>().EndGame += OnEndGameCall;

        SetTimer?.Invoke(initialScore, OneStarValue, TwoStarValue, ThreeStarValue); //Giving initial score value to score timer which is subscribed to the event

        OnReset(resetDuration);  //Reseting things
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

        //Resizing code
        if (resizable == true)     //Make sure min, max and resizeFactors are set if getting errors
        {
            //Checking if resize keys are pressed
            shrinkKeyPressed = Input.GetKey(KeyCode.S);
            growKeyPressed = Input.GetKey(KeyCode.D);

            if (shrinkKeyPressed == true && growKeyPressed == false)
            {
                float resizeAmount = 0;
                resizeAmount += Time.deltaTime / resizeFactor; //Indicating amount of resize

                sizeScale = this.gameObject.transform.localScale;   //Getting size of object

                if (sizeScale.x > minCharacterSize)    //Adjusting sizes if object is still bigger than min size
                {
                    //Adjusting size of object
                    sizeScale = new Vector3(sizeScale.x - resizeAmount, sizeScale.y - resizeAmount, sizeScale.z);
                    this.gameObject.transform.localScale = sizeScale;
                }
            }
            else if (growKeyPressed == true && shrinkKeyPressed == false)
            {
                float resizeAmount = 0;
                resizeAmount += Time.deltaTime / resizeFactor; //Indicating amount of resize

                sizeScale = this.gameObject.transform.localScale;

                if (sizeScale.x < maxCharacterSize)    //Adjusting sizes if object is still smaller than max size
                {
                    //Adjusting size of object
                    sizeScale = new Vector3(sizeScale.x + resizeAmount, sizeScale.y + resizeAmount, sizeScale.z);
                    this.gameObject.transform.localScale = sizeScale;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        //If left clicked
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.CanGrab == true)
        {
            Vector3 mousePos;   //Variable to keep track of mouse position
            mousePos = Input.mousePosition; //Getting value for mouse position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Getting values for start positions
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isBeingHeld = true; //Object being held by mouse

            //Playing clickdown sound
            AudioManager.instance.Play("ClickDown");

            //Changing border color alpha if there is a border
            if (characterBorder != null)
            {
                characterBorder.color = new Color32(255, 255, 255, MouseDownBorderAlpha);
            }

            if (GameManager.Instance.GameActive == false)   //If game has not started yet
            {
                StartGame?.Invoke();  //Invokes event if event is not null
                AudioManager.instance.Play("StartSound");   //Playing start sound
                AudioManager.instance.Play("TickSound");    //Playing tick sound
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.A) && GameManager.Instance.CanGrab == true)    //Same as OnMouseDown
        {
            Vector3 mousePos;   //Variable to keep track of mouse position
            mousePos = Input.mousePosition; //Getting value for mouse position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Getting values for start positions
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isBeingHeld = true; //Object being held by mouse

            //Playing clickdown sound
            AudioManager.instance.Play("ClickDown");

            //Changing border color alpha if there is a border
            if (characterBorder != null)
            {
                characterBorder.color = new Color32(255, 255, 255, MouseDownBorderAlpha);
            }

            if (GameManager.Instance.GameActive == false)   //If game has not started yet
            {
                StartGame?.Invoke();  //Invokes event if event is not null
                AudioManager.instance.Play("StartSound");   //Playing start sound
                AudioManager.instance.Play("TickSound");    //Playing tick sound
            }
        }
        else if (Input.GetKeyUp(KeyCode.A) && GameManager.Instance.CanGrab == true) //Same as OnMouseUp
        {
            isBeingHeld = false;    //Object not being held by mouse

            //Playing clickup sound
            AudioManager.instance.Play("ClickUp");

            //Changing border color alpha if there is a border
            if (characterBorder != null)
            {
                characterBorder.color = new Color32(255, 255, 255, MouseUpBorderAlpha);
            }
        }
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.GameActive == true)
        {
            isBeingHeld = false;    //Object not being held by mouse

            //Playing clickup sound
            AudioManager.instance.Play("ClickUp");

            //Changing border color alpha if there is a border
            if (characterBorder != null)
            {
                characterBorder.color = new Color32(255, 255, 255, MouseUpBorderAlpha);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line") || other.CompareTag("DangerBlock"))
        {
            collisionCount += 1;    //Adding to collision count
            if (other.CompareTag("DangerBlock") && GameManager.Instance.CanGrab == true)
            {
                if (other.GetComponent<Danger>().isSafe == false)   //If dangerblock is unsafe
                {
                    //Adjusting values
                    GameManager.Instance.Attempts += 1;

                    //Calling game end
                    EndGame?.Invoke(false, false);  //Invokes event if event is not null
                }
            }
        }
        else if (other.CompareTag("Finish") && GameManager.Instance.GameActive == true)
        {
            bool setHighScore = false;

            //Adjusting values
            GameManager.Instance.Attempts += 1;
            GameManager.Instance.Finishes += 1;
            GameManager.Instance.OldFinishScoresSum = GameManager.Instance.FinishScoresSum;
            GameManager.Instance.FinishScoresSum += GameManager.Instance.Score;
            GameManager.Instance.SessionFinishScoresSum += GameManager.Instance.Score;
            GameManager.Instance.SessionFinishes += 1;

            if (GameManager.Instance.Score > GameManager.Instance.Highscore)    //If score is higher than highscore
            {
                //Setting new highscore
                GameManager.Instance.OldHighscore = GameManager.Instance.Highscore;
                GameManager.Instance.Highscore = GameManager.Instance.Score;

                setHighScore = true;
            }

            //Calling game end
            EndGame?.Invoke(true, setHighScore);  //Invokes event if event is not null
        }
        else if (other.name == "LightUpCollider" && GameManager.Instance.CanGrab == true)
        {
            if (other.GetComponent<ColliderIdScript>().colliderIndex != 1)  //Not playing sound on first lightcollider
            {
                //Playing line hit sound effect
                AudioManager.instance.sounds[0].pitch += 0.1f;
                AudioManager.instance.Play("LineHitSound");
            }

            //Lighting up line
            other.transform.parent.GetComponent<LineScript>().LightUp(other.GetComponent<ColliderIdScript>().colliderIndex);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Line") || other.CompareTag("DangerBlock"))
        {
            collisionCount -= 1;    //Subtracting from collision count

            if (collisionCount == 0 && GameManager.Instance.GameActive == true)
            {
                //Adjusting values
                GameManager.Instance.Attempts += 1;

                //Calling game end
                EndGame?.Invoke(false, false);  //Invokes event if event is not null
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Lighting up the first line segment the character is in collision with
        if (other.name == "LightUpCollider" && GameManager.Instance.GameActive == false && GameManager.Instance.CanGrab == true && alreadyDone == false)
        {
            other.transform.parent.Find("LitLine").gameObject.SetActive(true);
            alreadyDone = true;
        }
    }

    private void OnGameEnd(bool finished, bool setHighScore)
    {
        isBeingHeld = false;    //Dropping out of hold

        //Playing gameover sound
        AudioManager.instance.Play("GameOverSound");
        //Stoping tick sound
        AudioManager.instance.StopPlaying("TickSound");
    }

    private void OnReset(float resetDuration)
    {
        if (firstReset == true) //First reset initiated from start
        {
            transform.position = initialCharacterPosition;
            transform.localScale = initialCharacterSize;

            GameManager.Instance.CanGrab = true;

            firstReset = false;
        }
        else if (firstReset == false)   //Not first reset
        {
            //Reseting character
            LeanTween.move(this.gameObject, initialCharacterPosition, resetDuration).setEaseInOutQuart();
            LeanTween.scale(this.gameObject, initialCharacterSize, resetDuration).setEaseInOutQuart();

            StartCoroutine(CanGrabReset(resetDuration));    //Coroutine for reseting whether object can be grabed
        }

        if (characterBorder != null)    //If there is a border
        {
            characterBorder.color = new Color32(255, 255, 255, MouseUpBorderAlpha); //Border color alpha
        }

        isBeingHeld = false;    //Is not in hold
        alreadyDone = false;    //Reseting

        //Setting initial Line hit pitch
        AudioManager.instance.sounds[0].pitch = 0.1f;
    }

    private void OnResetCall()
    {
        //Calling game reset
        ResetGame?.Invoke(resetDuration);
    }

    private void OnEndGameCall(bool finished, bool setHighScore)
    {
        //Adjusting values
        GameManager.Instance.Attempts += 1;

        //Calling endGame
        EndGame?.Invoke(finished, setHighScore);
    }   //Calls end game from timer

    private void LoadSkins()
    {
        PlayerPrefs.SetString("Trail", "Rainbow");
        PlayerPrefs.SetString("Skin", "Skin0");

        //Getting values from PlayerPrefs
        string skinName = PlayerPrefs.GetString("Skin", "DefaultSkin");
        string borderName = PlayerPrefs.GetString("Border", "DefaultBorder");
        string trailName = PlayerPrefs.GetString("Trail", "DefaultTrail");

        //Loading skin
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Skins/" + skinName);

        //Loading border
        if (borderName != "None")
        {
            var border = Resources.Load<GameObject>("Borders/Border");
            border = Instantiate(border, transform.position, transform.rotation, this.transform);    //Making the border object
            border.name = "Border";
            border.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Borders/Sprites/" + borderName); //Loading border sprite
            characterBorder = border.GetComponent<SpriteRenderer>();    //Setting up script reference to sprite renderer
        }

        //Loading trail
        if (trailName != "None")
        {
            var trail = Resources.Load<GameObject>("Trails/" + trailName);
            trail = Instantiate(trail, transform.position, transform.rotation); //Making the trail object
            trail.name = "Trail";
        }
    }

    IEnumerator CanGrabReset(float _resetDuration)  //Delayed so object can't be grabbed while reseting to initial position
    {
        yield return new WaitForSeconds(_resetDuration);
        GameManager.Instance.CanGrab = true;
    }
}