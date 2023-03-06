using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{

    public GameObject pause;
    public GameObject play;
    public GameObject home;


    //private void Start()
    //{
    //    play.SetActive(false);
    //    home.SetActive(false);
    //}

    public void PlayGame()
    {
        SceneManager.LoadScene("Spring");
    }

    public void SelectSpring()
    {
        SceneManager.LoadScene("Spring");
    }

    public void SelectSummer()
    {
        SceneManager.LoadScene("Summer");
    }

    public void SelectFall()
    {
        SceneManager.LoadScene("Fall");
    }

    public void SelectWinter()
    {
        SceneManager.LoadScene("Winter");
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public void Pause()
    {
        play.SetActive(true);
        home.SetActive(true);
        pause.SetActive(false);
        Debug.Log("Pause button clicked");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pause.SetActive(true);
        play.SetActive(false);
        home.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
