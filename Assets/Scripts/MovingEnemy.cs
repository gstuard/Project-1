using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{

    public float speed;
    public bool grounded;
    public LayerMask groundLayers;
    public float groundRayLength = 0.1f;
    public float groundRaySpread = 0.1f;
    public float ledgeTestLeft;
    public float ledgeTestRight;

    private Rigidbody2D rb2d;
    private int direction = 1;
   

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateGrounding();
        UpdateDirection();

        Vector2 vel = rb2d.velocity;
        vel.x = direction * speed;
        rb2d.velocity = vel;
        Debug.Log("Direction: " + direction);
    }

    int UpdateDirection()
    {

        Vector3 ledgeRayStartLeft = transform.position + Vector3.up * groundRayLength + Vector3.left * ledgeTestLeft;
        Vector3 ledgeRayStartRight = transform.position + Vector3.up * groundRayLength + Vector3.right * ledgeTestRight;

        RaycastHit2D hitLeft = Physics2D.Raycast(ledgeRayStartLeft, Vector2.down, groundRayLength * 2, groundLayers);
        RaycastHit2D hitRight = Physics2D.Raycast(ledgeRayStartRight, Vector2.down, groundRayLength * 2, groundLayers);

        Debug.DrawLine(ledgeRayStartLeft, ledgeRayStartLeft + Vector3.down * groundRayLength * 2, Color.red);
        Debug.DrawLine(ledgeRayStartRight, ledgeRayStartRight + Vector3.down * groundRayLength * 2, Color.red);

        if (hitLeft.collider == null)
        {
            direction = 1;
        }
        if (hitRight.collider == null)
        {
            direction = -1;
        }

        Debug.Log("Grounded: " + grounded);

        if (!grounded)
        {
            direction = 0;
            return 0;
        }

        if (direction == 0)
        {
            direction = 1;
        }
        return direction;
    }

    bool UpdateGrounding()
    {
        //change how these rays are set up because the draw lines show that they aren't on the ground, need to be
        //touching the floor

        Vector3 rayStart = transform.position + Vector3.up * groundRayLength;
        Vector3 rayStartLeft = transform.position + Vector3.up * groundRayLength + Vector3.left * groundRaySpread;
        Vector3 rayStartRight = transform.position + Vector3.up * groundRayLength + Vector3.right * groundRaySpread;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundRayLength * 2, groundLayers);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.down, groundRayLength * 2, groundLayers);
        RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.down, groundRayLength * 2, groundLayers);

        Debug.DrawLine(rayStart, rayStart + Vector3.down * groundRayLength * 2, Color.red);
        Debug.DrawLine(rayStartLeft, rayStartLeft + Vector3.down * groundRayLength * 2, Color.red);
        Debug.DrawLine(rayStartRight, rayStartRight + Vector3.down * groundRayLength * 2, Color.red);

        if (hit.collider != null || hitLeft.collider != null || hitRight.collider != null)
        {
            grounded = true;
            return true;
        }

        if (hit.collider == null)
        {
            Debug.Log("Hit null");
        }
        if (hitRight.collider == null)
        {
            Debug.Log("Hit right null");
        }
        if (hitLeft.collider == null)
        {
            Debug.Log("Hit left null");
        }

        grounded = false;
        return false;
    }
}
