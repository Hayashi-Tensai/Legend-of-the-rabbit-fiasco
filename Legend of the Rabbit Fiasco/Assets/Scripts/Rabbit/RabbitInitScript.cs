using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitInitScript : MonoBehaviour
{
    private Animator anim;
    public BoxCollider2D rabbitCollider;

	void Start ()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        anim.Play("Spawn");
        StartCoroutine(WaitForAnimation());
	}

	IEnumerator WaitForAnimation ()
    {
        yield return new WaitForSeconds(0.25f);
        GetComponent<RabbitAIScript>().enabled = true;
        rabbitCollider.enabled = true;
        yield return null;
    }
}
