using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SummaryScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private Image ScreenshotImage;
    [SerializeField] private TextMeshProUGUI FinishPercentageText;
    [SerializeField] private TextMeshProUGUI AttemptsText;
    [SerializeField] private TextMeshProUGUI FinishesText;
    [SerializeField] private TextMeshProUGUI HighscoreText;
    [SerializeField] private TextMeshProUGUI HighscoreDiffText;
    [SerializeField] private TextMeshProUGUI AvgFinishScoreText;
    [SerializeField] private TextMeshProUGUI AvgDiffText;
    [SerializeField] private TextMeshProUGUI SessAvgFinishScoreText;
    [SerializeField] private TextMeshProUGUI SessAvgDiffText;
    [SerializeField] private TextMeshProUGUI OneStarText;
    [SerializeField] private TextMeshProUGUI TwoStarText;
    [SerializeField] private TextMeshProUGUI ThreeStarText;

    [Header("Colors")]
    [SerializeField] private Color32 greenColor;
    [SerializeField] private Color32 redColor;
    [SerializeField] private Color32 invisibleColor;

    float _AvgFinishScore;
    decimal AvgFinishScore;
    float _AvgDiff;
    decimal AvgDiff;
    float _SessAvgFinishScore;
    decimal SessAvgFinishScore;
    float _SessAvgDiff;
    decimal SessAvgDiff;

    private Animator MainAnimator;

    private bool starValuesDisplayed = false;

    private void Awake()
    {        
        //Setting Canvas pos to screen
        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    void Start()
    {
        //Subscribing to events
        FindObjectOfType<PlayerScript1>().EndGame += Activated;
        FindObjectOfType<PlayerScript1>().ResetGame += UnActivated;

        //Getting animator component
        MainAnimator = GetComponent<Animator>();
    }

    void Activated(bool finished, bool setHighScore)
    {
        if (finished == true)
        {
            //Setting values to summary objects
            ScoreText.text = GameManager.Instance.Score.ToString("F2");
            //Hiding ScreenshotImage
            ScreenshotImage.enabled = false;

            //Setting number of stars
            if (GameManager.Instance.Score >= GameManager.Instance.ThreeStarValue)
            {
                MainAnimator.SetInteger("Stars", 3);
            }
            else if (GameManager.Instance.Score >= GameManager.Instance.TwoStarValue)
            {
                MainAnimator.SetInteger("Stars", 2);
            }
            else if (GameManager.Instance.Score >= GameManager.Instance.OneStarValue)
            {
                MainAnimator.SetInteger("Stars", 1);
            }
            else
            {
                //Code for when no star is achieved here
            }
        }
        else if (finished == false)
        {
            //Setting values to summary objects
            ScoreText.text = "DNF";
        }

        if (starValuesDisplayed == false)   //Whether star values have been set to display yet
        {
            //Displaying star values
            OneStarText.text = GameManager.Instance.OneStarValue.ToString("F2");
            TwoStarText.text = GameManager.Instance.TwoStarValue.ToString("F2");
            ThreeStarText.text = GameManager.Instance.ThreeStarValue.ToString("F2");

            starValuesDisplayed = true; //They are displayed
        }

        UpdateSummary(finished , setHighScore);    //Updating summary

        //Making summary menu activated
        MainAnimator.SetBool("Activated", true);
        if (finished == false){MainAnimator.SetTrigger("ScreenshotActivate");}  //Mainking Screenshot activated if didn't finish
    }

    void UnActivated(float resetDuration)
    {
        //Making summary menu unactivated
        MainAnimator.SetBool("Activated", false);

        //Starting reset coroutine
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(0.5f);  //Needs to be the same duration as the SummaryDisactivate animation

        //Reseting things
        ScreenshotImage.enabled = true;
        MainAnimator.SetInteger("Stars", 0);    //Reseting # of stars
    }

    void UpdateSummary(bool _finished, bool _SetNewHighscore)
    {
        if (GameManager.Instance.FinishScoresSum != 0 && GameManager.Instance.Finishes != 0 && GameManager.Instance.OldFinishScoresSum != 0)
        {
            //Calculating AvgFinishScore stuff
            _AvgFinishScore = GameManager.Instance.FinishScoresSum / (float)GameManager.Instance.Finishes;
            AvgFinishScore = decimal.Round((decimal)_AvgFinishScore, 2);
            _AvgDiff = (float)AvgFinishScore - GameManager.Instance.OldFinishScoresSum / ((float)GameManager.Instance.Finishes - 1);
            AvgDiff = decimal.Round((decimal)_AvgDiff, 2);
        }
        else if(_finished)
        {
            AvgFinishScore = decimal.Round((decimal)GameManager.Instance.Score, 2);
        }

        //Calculating SessAvgFinishScore stuff
        if (GameManager.Instance.SessionFinishScoresSum != 0 && GameManager.Instance.SessionFinishes != 0)
        {
            _SessAvgFinishScore = GameManager.Instance.SessionFinishScoresSum / (float)GameManager.Instance.SessionFinishes;
            SessAvgFinishScore = decimal.Round((decimal)_SessAvgFinishScore, 2);
            _SessAvgDiff = ((float)SessAvgFinishScore - (float)AvgFinishScore) / (float)AvgFinishScore * 100;   //Percent difference
            SessAvgDiff = decimal.Round((decimal)_SessAvgDiff, 3);
        }

        if (_finished)
        {
            if (AvgDiff >= 0)
            {
                //Diff was positive
                AvgDiffText.color = greenColor;
                AvgDiffText.text = "+" + AvgDiff.ToString("F2");
            }
            else
            {
                //Diff was negative
                AvgDiffText.color = redColor;
                AvgDiffText.text = AvgDiff.ToString("F2");
            }

            if (SessAvgDiff >= 0)
            {
                //Diff was positive
                SessAvgDiffText.color = greenColor;
                SessAvgDiffText.text = "+" + SessAvgDiff.ToString("F3") + "%";
            }
            else
            {
                //Diff was negative
                SessAvgDiffText.color = redColor;
                SessAvgDiffText.text = SessAvgDiff.ToString("F3") + "%";
            }
        }
        else
        {
            AvgDiffText.color = invisibleColor; //Hiding diff text
            SessAvgDiffText.color = invisibleColor; //Hiding diff text
        }

        //Updating summary text values
        AttemptsText.text = GameManager.Instance.Attempts.ToString();
        FinishesText.text = GameManager.Instance.Finishes.ToString();
        FinishPercentageText.text = decimal.Round((decimal)GameManager.Instance.Finishes / (decimal)GameManager.Instance.Attempts * 100, 2).ToString("F1") + "%";
        HighscoreText.text = GameManager.Instance.Highscore.ToString("F2");
        AvgFinishScoreText.text = AvgFinishScore.ToString("F2");
        SessAvgFinishScoreText.text = SessAvgFinishScore.ToString("F2");

        if (_SetNewHighscore == true)   //If new highscore was set
        {
            HighscoreDiffText.color = greenColor;   //Giving color
            HighscoreDiffText.text = "+" + (GameManager.Instance.Highscore - GameManager.Instance.OldHighscore).ToString("F2");
            TitleText.text = "NEW HIGHSCORE";
        }
        else    //If new highscore wasn't set
        {
            HighscoreDiffText.color = invisibleColor;   //making invisible
            HighscoreDiffText.text = "+00.00";
            TitleText.text = "SCORE";
        }

        //Saving new values to playerprefs
        GameManager.Instance.SetLevelValues();
    }
}
