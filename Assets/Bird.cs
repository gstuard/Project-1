using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public float original_speed;
    public float friction; // vestigial
    public float air_friction; // 1 is VERY low, 10 is avg, 20 is good
    public float jump_height;

    internal float speed;
    internal float current_jump_height;
    internal float flight_timer = 0;
    internal bool flying = false;
    internal float flight_angle = 0; // add gravity speed boost later
    internal float jump_timer = 0;

    internal float fall_speed;
    internal float normal_fall_speed = 10;
    internal float fast_fall_speed = 16;
    internal float normal_gravity = 2.25f;
    internal float fast_gravity = 5.5f;

    internal Rigidbody2D rb;
    internal SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        fall_speed = normal_fall_speed;
        speed = original_speed;
        current_jump_height = jump_height;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    Vector2 GetInputVector()
    {
        Vector2 input_vector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.DownArrow))
        {
            input_vector.y = -1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            input_vector.y = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input_vector.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input_vector.x = 1;
        }
        return input_vector;
    }


    bool IsGrounded() // make this bigger/add more rays
    {
        float distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f;
        Vector3 ray_start = new Vector3(transform.position.x, transform.position.y - distToGround, transform.position.z);
        RaycastHit2D floorHit = Physics2D.Raycast(ray_start, Vector2.down, 0.05f);
        return floorHit.collider != null;
    }


    void Jump() // make bird slightly faster in the air?
    {
        if (current_jump_height > rb.velocity.y || flight_timer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, current_jump_height);
            current_jump_height -= jump_height / 3;
            jump_timer = 0.1f; // this is vesgtigious
            //speed *= 9 / 8;
            flying = false;
        }
    }


    void JumpUpdate()
    {
        if (jump_timer > 0)
        {
            jump_timer -= Time.deltaTime;
        }
        if (IsGrounded())
        {
            if (jump_timer <= 0)
            {
                current_jump_height = jump_height;
            }
            jump_timer = 0;
            flight_timer = .9f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }
    }


    void GroundMove()
    {
        //speed = original_speed;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            // friction is now unused, would be used here
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

     
    void FastFall()
    {
        if (Input.GetKey(KeyCode.DownArrow) && rb.velocity.y < 0)
        {
            fall_speed = fast_fall_speed;
            rb.gravityScale = fast_gravity;
        }
        else
        {
            fall_speed = normal_fall_speed;
            rb.gravityScale = normal_gravity;
        }
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -fall_speed));
    }


    void AirMove()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float x_vel = Mathf.Max(-speed, rb.velocity.x - Time.deltaTime * air_friction);
            rb.velocity = new Vector2(x_vel, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float x_vel = Mathf.Min(speed, rb.velocity.x + Time.deltaTime * air_friction);
            rb.velocity = new Vector2(x_vel, rb.velocity.y);
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (rb.velocity.x > 0)
            {
                float x_vel = Mathf.Max(0, rb.velocity.x - Time.deltaTime * air_friction / 3);
                rb.velocity = new Vector2(x_vel, rb.velocity.y);
            }
            else if (rb.velocity.x < 0)
            {
                float x_vel = Mathf.Min(0, rb.velocity.x + Time.deltaTime * air_friction / 3);
                rb.velocity = new Vector2(x_vel, rb.velocity.y);
            }
        }

        FastFall();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        flight_timer = 0;
    }


    void Fly()
    {
        flying = true;
        rb.gravityScale = 0f;
        speed = original_speed * 1.425f;
        sr.color = Color.red;
    }


    void FlyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X) && flight_timer > 0)
        {
            Fly();
        }

        if (flight_timer > 0 && flying)
        {
            flight_timer -= Time.deltaTime;

            Vector2 input_vector = GetInputVector();

            if (input_vector != Vector2.zero)
            {
                Vector2 flight_vel = Vector2.Lerp(rb.velocity.normalized, input_vector, 0.035f);
                rb.velocity = flight_vel;
            }

            rb.velocity = rb.velocity.normalized;
            rb.velocity *= speed;
        } else 
        {
            rb.gravityScale = 2f;
            speed = original_speed; // add a slow down mechanic/keep speed for a sec
            sr.color = Color.blue;
        }

        if (flight_timer <= 0)
        {
            flying = false;
        }
    }


    void LifeUpdate()
    {
        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(-3, -1, 0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        JumpUpdate();
        FlyUpdate(); // can change this so its only called on second else, 3rd cond. below
        LifeUpdate();

        if (IsGrounded())
        {
            GroundMove();
        }
        else if (!flying)
        {
            AirMove();
        }
    }
}


// to do: add better walls, add short jump