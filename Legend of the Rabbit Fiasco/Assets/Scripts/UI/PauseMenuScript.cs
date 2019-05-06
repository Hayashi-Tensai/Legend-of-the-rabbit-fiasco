using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }
    }

    public void PauseButton()
    {
        if(Time.timeScale == 1)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManagerScript.instance.LoadScene(1);
    }
}
