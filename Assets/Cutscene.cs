using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{

    public GameObject bird;
    public GameObject maincamera;

    public float cutscene_trigger_x;

    internal float cutscene_time = 0f;
    
    // Update is called once per frame
    void Update()
    {
        if (bird.transform.position.x > cutscene_trigger_x)
        {
            bird.GetComponent<Bird>().rb.velocity = Vector2.zero;
            bird.GetComponent<Bird>().SetMoveLock(1f);

            if (cutscene_time < 2f)
            {
                maincamera.GetComponent<CameraController>().zoom = 1;
            }
            else if (cutscene_time < 5f)
            {
                maincamera.GetComponent<CameraController>().zoom = 0;
            }
            else if (cutscene_time < 7f)
            {
                maincamera.GetComponent<CameraController>().zoom = -1;
            }
            else
            {
                maincamera.GetComponent<CameraController>().zoom = 0;
                Destroy(transform.gameObject);
            }


            cutscene_time += Time.deltaTime;
        }
    }
}
