using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHitboxScript : MonoBehaviour
{
    public int hitboxDmg = 1;

    [HideInInspector]
    public float hitboxEnableTimer;
    private bool isHitboxOn = false;

    public float hitboxEnableDuration = 0.3f;

	private void Update ()
    {
		if (gameObject.activeSelf == true && isHitboxOn == false)
        {
            hitboxEnableTimer = hitboxEnableDuration;
            isHitboxOn = true;
        }

        if ((hitboxEnableTimer > 0.0f))
        {
            hitboxEnableTimer -= Time.deltaTime;
        }
        else 
        {
            isHitboxOn = false;
            PlayerControllerScript.instance.canPlayMoveAnimation = true;
            gameObject.SetActive(false);
            PlayerControllerScript.instance.ChangeBacktoIdle();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
                other.GetComponent<RabbitAIScript>().TakeDamage(hitboxDmg);
        }
    }
}
