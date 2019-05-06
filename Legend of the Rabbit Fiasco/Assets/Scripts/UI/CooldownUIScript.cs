using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIScript : MonoBehaviour
{
    public Image abilityOverlayImage;
    private float currentCooldownPercentage;

    public void SetCooldown(float timer, float cooldown)
    {
        if (timer != cooldown && !(timer <= 0.0f))
        {
            currentCooldownPercentage = timer / cooldown;
        }

        abilityOverlayImage.fillAmount = currentCooldownPercentage;
    }
}
