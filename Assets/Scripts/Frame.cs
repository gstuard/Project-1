using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public GameObject cameracontroller;
    public GameObject bird;
    public LayerMask birdlayer;

    public int frame_index;
    public float frameMaxX;
    public float frameMaxY;
    public float frameMinX;
    public float frameMinY;
    public Vector2 direction_to_next_frame;

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
        if (cameracontroller.GetComponent<CameraController>().current_frame == frame_index)
        {
            RaycastHit2D below_screen = Physics2D.Raycast(hitbox_origin, hitbox_direction, hitbox_length, birdlayer);
            Debug.DrawRay(hitbox_origin, hitbox_direction * hitbox_length, Color.red);
            if (below_screen.collider != null)
            {
                RestartLevel();
                bird.GetComponent<Bird>().Respawn();
            }

            if (direction_to_next_frame.x > 0)
            {
                if (bird.transform.position.x > frameMaxX)
                {
                    cameracontroller.GetComponent<CameraController>().ToFrame(frame_index + 1);
                }
                else cameracontroller.GetComponent<CameraController>().ToFrame(frame_index);
            }
            else if (direction_to_next_frame.x < 0)
            {
                if (bird.transform.position.x < frameMinX)
                {
                    cameracontroller.GetComponent<CameraController>().ToFrame(frame_index + 1);
                }
                else cameracontroller.GetComponent<CameraController>().ToFrame(frame_index);
            }

            else if (direction_to_next_frame.y > 0)
            {
                if (bird.transform.position.y > frameMaxY)
                {
                    cameracontroller.GetComponent<CameraController>().ToFrame(frame_index + 1);
                }
                else cameracontroller.GetComponent<CameraController>().ToFrame(frame_index);
            }
            else if (direction_to_next_frame.y < 0)
            {
                if (bird.transform.position.y < frameMinY)
                {
                    cameracontroller.GetComponent<CameraController>().ToFrame(frame_index + 1);
                }
                else cameracontroller.GetComponent<CameraController>().ToFrame(frame_index);
            }
            else Debug.LogError("Variable direction_to_next_frame (Vector2) is empty, so camera does not know which frame to point at.");
        }
    }
}
