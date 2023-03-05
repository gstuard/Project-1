using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCutscene : MonoBehaviour
{
    public GameObject bird;

    internal float stop_at = -100;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartCutscene()
    {
        stop_at = bird.transform.position.y + 30;
        transform.position = new Vector3(bird.transform.position.x, bird.transform.position.y - 10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < stop_at)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 20, 10);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -100, 10);
            stop_at = -90;
        }
    }
}
