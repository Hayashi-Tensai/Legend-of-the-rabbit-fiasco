using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScript : MonoBehaviour
{

    float time;

    void Start()
    {
        time = 0;
    }

    void Update()
    {
        if (time >= 8.5f)
        {
            SceneManagerScript.instance.LoadScene(3);
        }
        time += Time.deltaTime;
    }
}
