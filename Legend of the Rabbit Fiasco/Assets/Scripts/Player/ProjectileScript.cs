using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int projectileDmg;
    public float projectileSpeed;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<RabbitAIScript>().TakeDamage(projectileDmg);
        }
    }

    private void Update()
    {
        transform.localPosition += Vector3.right * projectileSpeed * Time.deltaTime;
    }
}
