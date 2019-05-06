using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotScript : MonoBehaviour
{
    public float projectileSpeed = 6.0f;
    private float lifetime = 3.0f;
    public int projectileDmg = 1;

    public Vector3 direction;

    public void InitSpawn(Vector3 _dir)
    {
        direction = _dir;
    }

    private void Update()
    {
        transform.position += direction * projectileSpeed * Time.deltaTime;

        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerControllerScript>().currentHealth -= projectileDmg;
            Destroy(gameObject);
        }
    }
}
