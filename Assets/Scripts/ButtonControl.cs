using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
