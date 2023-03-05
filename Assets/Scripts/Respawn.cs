using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public int frame_index;
    public GameObject maincamera;

    public void PanCamera()
    {
        maincamera.GetComponent<CameraController>().ToFrame(frame_index);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
