using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]private Animator transition;

    public void LoadScene(string nextScene)
    {
        //Deleting some PlayerPref values in prep for new session
        PlayerPrefs.DeleteKey(nextScene + "SessionFinishScoresSum");
        PlayerPrefs.DeleteKey(nextScene + "SessionFinishes");

        StartCoroutine(LoadTheScene(nextScene));  //Loading next scene
    }

    IEnumerator LoadTheScene(string _nextScene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(_nextScene);
    }
}
