using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float original_speed;
    public float friction; // vestigial
    public float air_friction; // 1 is VERY low, 10 is avg, 20 is good

    public float jump_height;
    public float[] jump_heights;
    internal int jump_index = 0;
    internal float jump_timer = 0;

    public LayerMask birdlayer;

    internal float speed;
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

    public GameObject current_frame;
    public Vector3 respawn = new Vector3(-1, -3, 0);

    internal Rigidbody2D rb;
    internal SpriteRenderer sr;

    public AudioClip clip;
    private AudioSource source;
    public GameObject maincamera;

    // Start is called before the first frame update
    void Start()
    {
        fall_speed = normal_fall_speed;
        speed = original_speed;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        flying = false; // end flight, add if statement to this for crystals soon probably

        if (collision.gameObject.CompareTag("Respawn")) // should we be using area effector?
        {
            Debug.Log("Respawn set");
            respawn = collision.gameObject.transform.position;
            respawn = new Vector3(respawn.x, respawn.y + 1, respawn.z);
            current_frame = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Enemy")) // should we be using area effector?
        {
            Respawn();
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
        float distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y - 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x, transform.position.y - distToGround, transform.position.z);
        RaycastHit2D floorHit = Physics2D.Raycast(ray_start, Vector2.down, 0.1f, birdlayer); 
        return floorHit.collider != null;
    }


    bool IsOnIce() // Ice!
    {
        float distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y - 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x, transform.position.y - distToGround, transform.position.z);
        RaycastHit2D floorHit = Physics2D.Raycast(ray_start, Vector2.down, 0.1f, birdlayer);
        if (floorHit.collider != null)
        {
            return floorHit.collider.CompareTag("Ice");
        }
        return false;
    }


    bool IsOnLeftWall()
    {
        float distToWall = GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x - distToWall, transform.position.y, transform.position.z);
        RaycastHit2D wallHit = Physics2D.Raycast(ray_start, Vector2.right, 0.1f, birdlayer);
        return wallHit.collider != null;
    }

    bool IsOnRightWall() // consolidate to is on wall?
    {
        float distToWall = GetComponent<CapsuleCollider2D>().bounds.extents.x + 0.05f;
        Vector3 ray_start = new Vector3(transform.position.x + distToWall, transform.position.y, transform.position.z);
        RaycastHit2D wallHit = Physics2D.Raycast(ray_start, Vector2.left, 0.1f, birdlayer);
        return wallHit.collider != null;
    }


    void Jump() // make bird slightly faster in the air?
    {
        if (flying) // this happens when the raycast has connected but there hasnt been a collision
        {
            rb.velocity = new Vector2(rb.velocity.x * 2, jump_height * 1.4f);
            speed *= 2f; // to do: speed magnifier var

            flying = false;
            flight_timer = 1f;
        }
        else if (jump_height > rb.velocity.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump_height);
            //speed *= 9 / 8;
        }
    }


    void WallJump()
    {
        float x_vel;
        speed *= 1.1f;
        if (IsOnRightWall())
        {
            x_vel = -speed;
        }
        else x_vel = speed; // if on left wall

        move_lock = 0.08f;
        jump_index = 0;
        rb.velocity = new Vector2(x_vel, jump_height * .75f); // how high should wall jump be?
    }


    void AirJump()
    {
        if (jump_index < jump_heights.Length)
        {
            if (flying)
            {
                flying = false;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y) / 2 + jump_heights[jump_index]);
                if (jump_heights[jump_index] < Mathf.Max(0, rb.velocity.y) / 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y - jump_heights[jump_index]) * 2);
                }
                jump_index++;
            }
            else if (jump_heights[jump_index] > rb.velocity.y)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump_heights[jump_index]);
                jump_index++;
            }
        }
    }


    void ShortHop()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 3);
    }


    void JumpUpdate()
    {
        if (IsGrounded())
        {
            jump_index = 0;
            flight_timer = .9f;
            if (Input.GetKeyDown(KeyCode.C))
            {
                jump_timer = 0.1f;
                Jump();
                source.PlayOneShot(clip);
            }
        }
        if (jump_timer > 0)
        {
            jump_timer -= Time.deltaTime;
            if (jump_timer <= 0)
            {
                if (!Input.GetKey(KeyCode.C))
                {
                    ShortHop();
                    source.PlayOneShot(clip);
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (!IsOnLeftWall() && !IsOnRightWall())
            {
                AirJump();
                source.PlayOneShot(clip);
            }
            else
            {
                WallJump();
                source.PlayOneShot(clip);
            }
        }      
    }


    void GroundMove()
    {
        if (!IsOnIce())
        {

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed * 0.8f, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed * 0.8f, rb.velocity.y);
        }
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            // friction is now unused, would be used here
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (move_lock > 0) // this is a short term fix
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


    void WallCling()
    {
        if (IsOnRightWall())
        {
            speed = original_speed;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                fall_speed = 2f;
            }
        }
        if (IsOnLeftWall())
        {
            speed = original_speed;
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
    }


    void Fly()
    {
        flying = true;
        rb.gravityScale = 0f;
        speed = original_speed * 1.525f;
        sr.color = Color.red;

        Vector2 input_vector = GetInputVector();
        if (input_vector != Vector2.zero)
        {
            rb.velocity = input_vector;
        }
    }


    void FlyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X) && flight_timer > 0 && !flying)
        {
            Fly();
        }

        if (flying) 
        {
            flight_timer -= Time.deltaTime;

            Vector2 input_vector = GetInputVector();

            if (input_vector != Vector2.zero)
            {
                Vector2 temp_vel = rb.velocity;
                Vector2.SmoothDamp(Vector2.zero, input_vector, ref temp_vel, 0.3f);
                rb.velocity = temp_vel;
            }

            rb.velocity = rb.velocity.normalized;
            rb.velocity *= speed;
        } else 
        {
            rb.gravityScale = normal_gravity;
            sr.color = Color.blue;
        }

        if (flight_timer <= 0)
        {
            flying = false;
        }
    }


    public void Respawn()
    {
        move_lock = 0.95f;
        sr.color = Color.clear;
        // bird does death animation here, next line would have to change, use a coroutine?
        transform.position = respawn;
        current_frame.GetComponentInParent<Frame>().RestartLevel(); // fix this?
        current_frame.GetComponent<Respawn>().PanCamera();
        GetComponentInChildren<RespawnCutscene>().StartCutscene();
    }


    void LifeUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
    }


    void PhysUpdate()
    {
        if (speed > original_speed && !flying)
        {
            speed -= Time.deltaTime * air_friction;
        }
        //if (speed < original_speed)
        //{
        //    speed = original_speed; // this removes slowness...
        //}

        if (move_lock > 0)
        {
            move_lock -= Time.deltaTime;
            if (move_lock > 0.1f)
            {
                sr.color = Color.clear;
            }
        }
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