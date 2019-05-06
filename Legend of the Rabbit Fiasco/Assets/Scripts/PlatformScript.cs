using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private float rotateOnTimer = 0.0f;
    private float rotateOnTimerAmount = 0.3f;
    private bool rotateOn = false;

    private PlatformEffector2D platformEffector;

    private void Start()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if (platformEffector.rotationalOffset == 180 && !rotateOn)
        {
            rotateOnTimer = rotateOnTimerAmount;
            rotateOn = true;
        }

        if (rotateOnTimer > 0.0f)
        {
            rotateOnTimer -= Time.deltaTime;
        }
        else if (rotateOnTimer <= 0.0f && rotateOn)
        {
            platformEffector.rotationalOffset = 0;
            rotateOn = false;
        }
    }
}
