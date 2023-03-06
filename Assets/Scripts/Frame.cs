using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public GameObject bird;
    public LayerMask birdlayer;

    public float frameMaxX;
    public float frameMaxY;
    public float frameMinX;
    public float frameMinY;

    public Vector2 end_ray_origin;
    public Vector2 end_ray_direction;
    public float end_ray_length;
    public int frame_index;

    public GameObject cameracontroller;

    public Vector2 hitbox_origin;
    public Vector2 hitbox_direction;
    public float hitbox_length; 


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void RestartLevel()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Leaf>() != null)
            {
                child.gameObject.GetComponent<Leaf>().Respawn();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit2D below_screen = Physics2D.Raycast(hitbox_origin, hitbox_direction, hitbox_length, birdlayer);
        Debug.DrawRay(hitbox_origin, hitbox_direction * hitbox_length, Color.red);
        if (below_screen.collider != null)
        {
            RestartLevel();
            bird.GetComponent<Bird>().Respawn();
        }

        RaycastHit2D end_ray = Physics2D.Raycast(end_ray_origin, end_ray_direction, end_ray_length, birdlayer);
        Debug.DrawRay(end_ray_origin, end_ray_direction * end_ray_length, Color.green);
        if (end_ray.collider != null)
        {
            if (end_ray.collider.CompareTag("Player")) {
                if (bird.transform.position.x < frameMaxX && bird.transform.position.y < frameMaxY)
                {
                    cameracontroller.GetComponent<CameraController>().ToFrame(frame_index);
                }
                else cameracontroller.GetComponent<CameraController>().ToFrame(frame_index + 1);
            }
        }
    }
}
