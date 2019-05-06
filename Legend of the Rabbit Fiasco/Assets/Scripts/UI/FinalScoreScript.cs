using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreScript : MonoBehaviour
{
    public Text finalScoreText;

    private void Start()
    {
        finalScoreText.text = "Final " + GameDataScript.instance.finalScoreText;
    }
}
