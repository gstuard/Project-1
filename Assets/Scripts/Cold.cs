using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : MonoBehaviour
{

    public GameObject bird;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered frozen zone!");
        bird.GetComponent<Bird>().in_freeze_zone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bird.GetComponent<Bird>().in_freeze_zone = false;
    }
}
