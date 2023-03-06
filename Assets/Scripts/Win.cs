using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    private BoxCollider2D bc;

    public string nextScene;

    public float timeTillNextScene;

    public bool win = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            win = true;
        }
    }
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    void Update()
    {
        if (win)
        {
            timeTillNextScene -= Time.deltaTime;
            if (timeTillNextScene <= 0)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }

    }
}
