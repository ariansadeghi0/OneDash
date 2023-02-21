using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public event Action<bool, bool> EndGame;

    [SerializeField] private Animator star1;
    [SerializeField] private Animator star2;
    [SerializeField] private Animator star3;

    [SerializeField] private GameObject scoreGameObject;
    private TextMeshProUGUI scoreText;

    private float oneStarValue;
    private float twoStarValue;
    private float threeStarValue;
    private bool done1 = false; //For keeping track
    private bool done2 = false;
    private bool done3 = false;

    private float initialScore;
    private float _score;
    private float score
    {
        get{
            return _score;
            }
        set{
            if (value <= 0){
                _score = 0;
            }
            else if (value >= initialScore){
                _score = initialScore;
            }
            else{
                _score = value;
            }
        }
    }

    private void Awake()
    {
        FindObjectOfType<PlayerScript1>().SetTimer += TimerSetup;   //Subscribing to event
    }

    private void TimerSetup(float _initialScore, float _OneStarValue, float _TwoStarValue, float _ThreeStarValue)    //Is called at start, use this over start method
    {
        //Setting values
        initialScore = _initialScore;
        oneStarValue = _OneStarValue;
        twoStarValue = _TwoStarValue;
        threeStarValue = _ThreeStarValue;

        FindObjectOfType<PlayerScript1>().ResetGame += ResetTimer;  //Subscribing to event

        scoreText = scoreGameObject.GetComponent<TextMeshProUGUI>();    //Getting text box
        score = initialScore;   //Setting score to initial value
        scoreText.text = score.ToString("F2");  //Displaying score
    }

    void Update()
    {
        if (GameManager.Instance.GameActive == true)
        {
            score -= Time.deltaTime;    //Subtracting from 
            GameManager.Instance.Score = score;
            scoreText.text = score.ToString("F2");  //Displaying score

            if (score < threeStarValue && done3 == false)
            {
                star3.SetBool("StarOut", true);    //Animating star out
                AudioManager.instance.Play("StarOut");  //Playing sound

                done3 = true;
            }
            else if (score < twoStarValue && done2 == false)
            {
                star2.SetBool("StarOut", true);    //Animating star out
                AudioManager.instance.Play("StarOut");  //Playing sound

                done2 = true;
            }
            else if (score < oneStarValue && done1 == false)
            {
                star1.SetBool("StarOut", true);    //Animating star out
                AudioManager.instance.Play("StarOut");  //Playing sound

                done1 = true;
            }
            else if (score <= 0) //If time runs out
            {
                //Calling EndGame
                EndGame?.Invoke(false, false);
            }
        }
    }

    private void ResetTimer(float resetDuration)
    {
        //Animating stars back in
        star1.SetBool("StarOut", false);
        star2.SetBool("StarOut", false);
        star3.SetBool("StarOut", false);
        //Setting bools to original value
        done1 = false;
        done2 = false;
        done3 = false;

        score = initialScore;   //Setting score to initial value
        scoreText.text = score.ToString("F2");  //Displaying score
    }
}
