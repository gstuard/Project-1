using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private BoxCollider2D bc;

    public float windSpeed;
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bird"))
        {
            Rigidbody2D rbOther = other.gameObject.GetComponent<Rigidbody2D>();
            if (rbOther != null)
            {
                rbOther.gravityScale = windSpeed;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bird"))
        {
            Rigidbody2D rbOther = other.gameObject.GetComponent<Rigidbody2D>();
            if (rbOther != null)
            {
                rbOther.gravityScale = 1f;
            }
        }
    }
}
