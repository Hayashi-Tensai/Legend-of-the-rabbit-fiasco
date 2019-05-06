using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataScript : MonoBehaviour
{
    public enum Difficulty
    {
        EASY,
        REGULAR,
        HARD
    }
    [HideInInspector]
    public string finalScoreText;
    public Difficulty difficulty;

    public static GameDataScript instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
}
