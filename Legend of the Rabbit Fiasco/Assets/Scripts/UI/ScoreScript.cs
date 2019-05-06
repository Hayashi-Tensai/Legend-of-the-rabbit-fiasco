using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance { get; set; }

    private int totalScore = 0;

    public Text scoreText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }

    private void Update()
    {
        GameDataScript.instance.finalScoreText = scoreText.text;
    }

    public void IncreaseScore(int score)
    {
        totalScore += score;
        scoreText.text = "Score: " + totalScore.ToString();
    }
}
