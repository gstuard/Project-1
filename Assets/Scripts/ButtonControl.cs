using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
