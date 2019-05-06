using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    private float timer;
    public Text timerText;

    private void Update()
    {
        timer = 60.0f - RabbitSpawnScript.instance.elaspedTime;
        timerText.text = "Timer: " + ((int)(Mathf.Round(timer * 100f) / 100f)).ToString();
    }
}
