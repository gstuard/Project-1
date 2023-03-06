using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlock : MonoBehaviour
{

    public GameObject bird;
    public float show_y;
    public float destroy_y;

    internal SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bird.transform.position.y < show_y)
        {
            sr.color = Color.white;
        }
        if (bird.transform.position.y < destroy_y)
        {
            Destroy(transform.gameObject);
        }
    }
}
