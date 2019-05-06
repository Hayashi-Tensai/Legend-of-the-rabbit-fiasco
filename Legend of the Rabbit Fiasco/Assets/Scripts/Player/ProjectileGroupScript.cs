using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGroupScript : MonoBehaviour
{
    private float lifetime = 3.0f;

    private Vector2 target;
    private Vector2 normalizedTarget;
    private Vector2 normalizedSouce;

    /*
    private Vector3 initDir;
    private Vector3 destinationDir;
    private Vector3 direction;
    */

    private void Start()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan((target.y - PlayerControllerScript.instance.transform.position.y) /
            (target.x - PlayerControllerScript.instance.transform.position.x)) * Mathf.Rad2Deg;
        
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x <= PlayerControllerScript.instance.transform.position.x)
        {
            angle = 180.0f + angle;
        }
        Debug.Log(angle);

        transform.eulerAngles = new Vector3(0, 0, angle);

        /*
        initDir = transform.position;
        destinationDir = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
        direction = destinationDir - initDir;
        direction.Normalize();
        */
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
            Destroy(gameObject);
    }
}
