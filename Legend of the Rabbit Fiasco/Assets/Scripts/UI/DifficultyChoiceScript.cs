using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyChoiceScript : MonoBehaviour
{
	public void ChooseDifficultyButton (int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                GameDataScript.instance.difficulty = GameDataScript.Difficulty.EASY;
                break;
            case 2:
                GameDataScript.instance.difficulty = GameDataScript.Difficulty.REGULAR;
                break;
            case 3:
                GameDataScript.instance.difficulty = GameDataScript.Difficulty.HARD;
                break;
        }
        SceneManagerScript.instance.LoadScene(2);
    }
}
