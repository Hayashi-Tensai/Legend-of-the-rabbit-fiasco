using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image healthBarImage;
    private float currentHealthPercentage;

    private void Start()
    {
        currentHealthPercentage = PlayerControllerScript.instance.currentHealth / PlayerControllerScript.instance.maxHealth;
    }

    private void Update()
    {
        SetHealth();
    }

    public void SetHealth()
    {
         currentHealthPercentage = (float)PlayerControllerScript.instance.currentHealth / (float)PlayerControllerScript.instance.maxHealth;
         healthBarImage.fillAmount = currentHealthPercentage;
    }
}
