using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

    public static SceneManagerScript instance { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void LoadScene(int scene)
    {
        switch(scene)
        {
            case 1:
                SceneManager.LoadScene("Main Menu");
                break;
            case 2:
                SceneManager.LoadScene("Story");
                break;
            case 3:
                SceneManager.LoadScene("Main Scene");
                break;
            case 4:
                SceneManager.LoadScene("Tutorial");
                break;
            case 5:
                SceneManager.LoadScene("Credits");
                break;
            case 6:
                SceneManager.LoadScene("Lose");
                break;
            case 7:
                SceneManager.LoadScene("Win");
                break;
            case 8:
                SceneManager.LoadScene("Difficulty Menu");
                break;
            case 99:
                Application.Quit();
                break;
        }
    }
}
