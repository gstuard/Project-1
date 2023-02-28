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
    internal float jump_timer = 0;
    internal float flight_timer = 0;
    internal float flight_angle = 0; // add gravity speed boost later
    internal bool flying = false;
    internal float move_lock = 0f;
    //internal bool move_lock = false;

    internal float fall_speed;
    internal float normal_fall_speed = 10;
    internal float fast_fall_speed = 16;
    internal float normal_gravity = 2.25f;
    internal float fast_gravity = 5.5f;

    public Vector3 respawn = new Vector3(-1, -3, 0);

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        flight_timer = 0; // end flight, add if statement to this for crystals soon probably

        if (collision.gameObject.CompareTag("Respawn")) // should we be using area effector?
        {
            Debug.Log("Respawn set");
            respawn = collision.gameObject.transform.position;
            respawn = new Vector3(respawn.x, respawn.y + 1, respawn.z);
            Debug.Log(respawn);
        }
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
        float distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x, transform.position.y - distToGround, transform.position.z);
        RaycastHit2D floorHit = Physics2D.Raycast(ray_start, Vector2.down, 0.05f); // should debug.ray this to make sure
        return floorHit.collider != null;
    }


    bool IsOnRightWall()
    {
        float distToWall = GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x + distToWall, transform.position.y, transform.position.z);
        RaycastHit2D wallHit = Physics2D.Raycast(ray_start, Vector2.right, 0.05f); // should debug.ray this to make sure
        return wallHit.collider != null;
    }

    bool IsOnLeftWall() // consolidate to is on wall?
    {
        float distToWall = GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x - distToWall, transform.position.y, transform.position.z);
        RaycastHit2D wallHit = Physics2D.Raycast(ray_start, Vector2.left, 0.05f); // should debug.ray this to make sure
        return wallHit.collider != null;
    }


    void Jump() // make bird slightly faster in the air?
    {
        Debug.Log("Jump Call");
        if (flying && IsGrounded()) // this happens when the raycast has connected but there hasnt been a collision
        {
            Debug.Log("Tech!!");
            rb.velocity = new Vector2(rb.velocity.x * 2, jump_height * 1.4f);
            speed *= 2f; // to do: speed magnifier var

            current_jump_height = jump_height;
            flying = false;
            flight_timer = 0.9f;
        }
        else if (current_jump_height > rb.velocity.y || flight_timer > 0)
        {
            if (jump_timer < 0.05f && IsGrounded())
            {
                Debug.Log("Shorthop");
                rb.velocity = new Vector2(rb.velocity.x, (3/4) * current_jump_height);
            }
            else rb.velocity = new Vector2(rb.velocity.x, current_jump_height);
            current_jump_height -= jump_height / 3;
            //speed *= 9 / 8;
            flying = false;
        }
    }


    void WallJump()
    {
        Debug.Log("Walljump Call");
        float x_vel = 0;
        speed *= 1.1f;
        if (IsOnRightWall())
        {
            x_vel = -speed;
        }
        else x_vel = speed; // if on left wall

        move_lock = 0.1f;
        rb.velocity = new Vector2(x_vel, current_jump_height);
        current_jump_height -= jump_height / 3;
        flying = false;
    }


    void JumpUpdate()
    {
        if (IsGrounded())
        {
            current_jump_height = jump_height;
            flight_timer = .9f;
            if (Input.GetKey(KeyCode.C))
            {
                jump_timer += Time.deltaTime;
            }
            else jump_timer = 0;

            if (jump_timer > 0.05f || Input.GetKeyUp(KeyCode.C))
            {
                Jump();
                jump_timer = 0;
                return;
            }
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (!IsOnLeftWall() && !IsOnRightWall())
            {
                Jump();
            }
            else
            {
                WallJump();
            }
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


    void WallCling()
    {
        if (IsOnRightWall())
        {
            current_jump_height = jump_height;
            speed = original_speed;
            //rb.velocity = new Vector2(0, rb.velocity.y); // do i need this?
            if (Input.GetKey(KeyCode.RightArrow))
            {
                fall_speed = 2f;
            }
        }
        if (IsOnLeftWall())
        {
            current_jump_height = jump_height;
            speed = original_speed;
            //rb.velocity = new Vector2(0, rb.velocity.y); // do i need this?
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                fall_speed = 2f;
            }
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

        WallCling();

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -fall_speed));
    }


    void AirMove()
    {
        if (move_lock <= 0)
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
        else move_lock -= Time.deltaTime;
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

        if (flying) // CHANGED
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
            //speed = original_speed; // add a slow down mechanic/keep speed for a sec
            sr.color = Color.blue;
        }

        if (flight_timer <= 0)
        {
            flying = false;
        }
    }


    void LifeUpdate()
    {
        //if (transform.position.y < -20f)
        //{
        //    transform.position = respawn;
        //}

        if (Input.GetKey(KeyCode.R))
        {
            transform.position = respawn;
        }
    }


    void PhysUpdate()
    {
        if (speed > original_speed && !flying)
        {
            speed -= Time.deltaTime * air_friction;
        }
        // to do grounded timer

        
    }


    // Update is called once per frame
    void Update()
    {
        JumpUpdate();
        FlyUpdate(); // can change this so its only called on second else, 3rd cond. below
        LifeUpdate();
        PhysUpdate();

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