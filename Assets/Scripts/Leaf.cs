using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float fall_speed;
    public float high_height;
    public float low_height;

    internal Rigidbody2D rb;
    internal SpriteRenderer sr;

    internal Vector2 start_pos;

    internal float movement_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        start_pos = rb.position;
    }


    public void Respawn()
    {
        rb.position = start_pos;
        rb.velocity = new Vector2(0, 0);
        movement_timer = 0;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        movement_timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement_timer > 0)
        {
            movement_timer -= Time.deltaTime;
            if (movement_timer > .15)
            {
                
            }
            else
            {
                rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, fall_speed * 2f));
            }
        }

        else
        {
            rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, -fall_speed)); 
        }

        if (rb.position.y < low_height)
        {
            rb.position = new Vector2(rb.position.x, high_height);
        }
    }
}
