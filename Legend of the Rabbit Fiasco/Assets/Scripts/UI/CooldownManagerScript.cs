using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManagerScript : MonoBehaviour
{
    public CooldownUIScript firstAbilityIcon;
    PlayerControllerScript player;

    private void Start()
    {
        player = PlayerControllerScript.instance;
    }

    private void Update()
    {
        if (player != null)
        {
            firstAbilityIcon.SetCooldown(player.firstAbilityTimer, player.firstAbilityCooldown);
        }  
    }
}
