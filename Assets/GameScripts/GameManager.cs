using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            //creating instance if instance doesn't exist
            if (_instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public bool GameActive { get; set; }    //If the game is active
    public bool CanGrab { get; set; }  //If character can be grabbed
    public float Score { get; set; }    //The score
    public float Highscore { get; set; }    //The highscore
    public float OldHighscore { get; set; } //The previous highscore
    public int Attempts { get; set; } //Number of attempts for this level
    public int Finishes { get; set; } //Number of finishes for this level
    public float FinishScoresSum { get; set; }    //The sum of all finish scores, used with finishes to calculate avg finish score
    public float OldFinishScoresSum { get; set; }
    public float SessionFinishScoresSum { get; set; }
    public int SessionFinishes { get; set; }
    public float OneStarValue { get; set; }
    public float TwoStarValue { get; set; }
    public float ThreeStarValue { get; set; }
    public string LastScene { get; set; }

    void Awake()
    {
        _instance = this;

        //REMOVE THIS LATER
        //DeleteCurrentLevelValues();
        //PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "SessionFinishScoresSum");
        //PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "SessionFinishes");
    }

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;   //Getting name of current scene
        //Checking for scenes since some scenes don't have the object in search
        try
        {
            FindObjectOfType<PlayerScript1>().ResetGame += ResetValues; //Subscribing to event
            FindObjectOfType<PlayerScript1>().StartGame += GameStarted; //Subscribing to event
            FindObjectOfType<PlayerScript1>().EndGame += GameEnded; //Subscribing to event
        }
        catch{}

        //Getting level values
        GetLevelValues();

        ResetValues(0f);  //Setting values to reset default
    }

    void GameStarted()
    {
        GameActive = true;
        CanGrab = true;
    }

    void GameEnded(bool finished, bool setHighScore)
    {
        GameActive = false;
        CanGrab = false;
    }

    void ResetValues(float resetDuration)
    {
        GameActive = false;
        //CanGrab will be set by player script
    }

    void GetLevelValues()
    {
        //Getting values
        OldHighscore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "OldHighscore", 0f);
        Highscore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "Highscore", 0f);
        Attempts = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Attempts", 0);
        Finishes = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Finishes", 0);
        FinishScoresSum = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "FinishScoresSum", 0f);
        OldFinishScoresSum = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "OldFinishScoresSum", 0f);
        SessionFinishScoresSum = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "SessionFinishScoresSum", 0f);
        SessionFinishes = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SessionFinishes", 0);
    }

    public void SetLevelValues()
    {
        //Setting values
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "OldHighscore", OldHighscore);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "Highscore", Highscore);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Attempts", Attempts);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Finishes", Finishes);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "FinishScoresSum", FinishScoresSum);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "OldFinishScoresSum", OldFinishScoresSum);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "SessionFinishScoresSum", SessionFinishScoresSum);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SessionFinishes", SessionFinishes);
        //Saving
        PlayerPrefs.Save();
    }

    public void SetStarValues(float _oneStarValue, float _twoStarValue, float _threeStarValue) //Called by player script at awake
    {   
        //Setting star values
        OneStarValue = _oneStarValue;
        TwoStarValue = _twoStarValue;
        ThreeStarValue = _threeStarValue;
    }

    public void LoadScene(string sceneName)
    {
        //Getting last scene (current scene)
        LastScene = SceneManager.GetActiveScene().name;

        //Trying to find levelLoader
        try
        {
            //Passing scene to load to levelLoader
            FindObjectOfType<LevelLoader>().LoadScene(sceneName);
        }
        catch
        {
            Debug.Log("LevelLoader was not found");
        }
    }

    #region Tools
    public static void DeleteCurrentLevelValues()
    {
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "OldHighscore");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "Highscore");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "Attempts");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "Finishes");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "FinishScoresSum");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "OldFinishScoresSum");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "SessionFinishScoresSum");
        PlayerPrefs.DeleteKey(SceneManager.GetActiveScene().name + "SessionFinishes");
    }

    public static void DeleteSkinsValues()
    {
        PlayerPrefs.DeleteKey("Skin");
        PlayerPrefs.DeleteKey("Border");
        PlayerPrefs.DeleteKey("Trail");
    }

    public static void DeleteAllValues()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion
}
