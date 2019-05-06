using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitHitBoxScript : MonoBehaviour
{
    public RabbitAIScript parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
           parent.isHit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            parent.isHit = false;
    }
}
