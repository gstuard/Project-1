using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public float speed;
    public float friction; // 1 is low, 10 is high

    internal Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(- speed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x - (Time.deltaTime * friction), rb.velocity.y);
        }
    }
}
