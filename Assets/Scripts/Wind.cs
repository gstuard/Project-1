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
        if (other.gameObject.CompareTag("Player"))
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
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rbOther = other.gameObject.GetComponent<Rigidbody2D>();
            if (rbOther != null)
            {
                rbOther.gravityScale = 1f;
            }
        }
    }



    //alternative maybe better way below???

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log("Object is in trigger");
    //    Vector3 position = transform.position;
    //    Vector3 targetPosition = other.transform.position;
    //    Vector3 direction = targetPosition - position;
    //    direction.Normalize();
    //    int moveSpeed = 10;
    //    other.transform.position += direction * moveSpeed * Time.deltaTime;

    //}
}
