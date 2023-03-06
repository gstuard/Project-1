using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float globalMaxX;
    public float globalMaxY;
    public float globalMinX;
    public float globalMinY;

    internal float tempMaxX;
    internal float tempMaxY;
    internal float tempMinX;
    internal float tempMinY;

    internal int zoom = 0;
    internal float original_size = 5;

    public int current_frame;
    public GameObject[] frames;


    void Start()
    {
        ToFrame(current_frame);
    }


    void ZoomOut()
    {
        Camera.current.orthographicSize += Time.deltaTime * 2.7f;
    }


    void ZoomIn()
    {
        Camera.current.orthographicSize -= Time.deltaTime * 2.7f;
        Camera.current.orthographicSize = Mathf.Max(original_size, Camera.current.orthographicSize);
    }


    public void ToFrame(int frame_index) 
    {
        tempMaxX = frames[frame_index].GetComponent<Frame>().frameMaxX;
        tempMaxY = frames[frame_index].GetComponent<Frame>().frameMaxY;
        tempMinX = frames[frame_index].GetComponent<Frame>().frameMinX;
        tempMinY = frames[frame_index].GetComponent<Frame>().frameMinY;
        current_frame = frame_index;
    }


    void SwapFrames()
    {
        if (target.position.x > globalMaxX)
        {
            globalMaxX += 18;
            globalMinX += 18;
        }
        if (target.position.x < globalMinX)
        {
            globalMaxX -= 18;
            globalMinX -= 18;
        }
        if (target.position.y > globalMaxY)
        {
            globalMaxY += 10;
            globalMinY += 10;
        }
        if (target.position.y < globalMinY)
        {
            globalMaxY -= 10;
            globalMinY -= 10;
        }
    }


    void Update()
    {
        globalMaxX = Mathf.Lerp(globalMaxX, tempMaxX, Time.deltaTime * 4);
        globalMaxY = Mathf.Lerp(globalMaxY, tempMaxY, Time.deltaTime * 4);
        globalMinX = Mathf.Lerp(globalMinX, tempMinX, Time.deltaTime * 4);
        globalMinY = Mathf.Lerp(globalMinY, tempMinY, Time.deltaTime * 4);

        Vector3 start = transform.position;
        Vector3 goal = target.position + new Vector3(0.0f, 0.0f, -10);
        float t = Time.deltaTime * speed;
        Vector3 newPosition = Vector3.Lerp(start, goal, t);
        float maxX = globalMaxX - Camera.main.orthographicSize * Camera.main.aspect;
        float maxY = globalMaxY - Camera.main.orthographicSize;
        float minX = globalMinX + Camera.main.orthographicSize * Camera.main.aspect;
        float minY = globalMinY + Camera.main.orthographicSize;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;

        if (zoom < 0)
        {
            ZoomIn();
        }
        if (zoom > 0)
        {
            ZoomOut();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(globalMinX, globalMinY, 0.0f), new Vector3(globalMaxX, globalMinY, 0.0f));
        Gizmos.DrawLine(new Vector3(globalMinX, globalMaxY, 0.0f), new Vector3(globalMaxX, globalMaxY, 0.0f));
        Gizmos.DrawLine(new Vector3(globalMinX, globalMinY, 0.0f), new Vector3(globalMinX, globalMaxY, 0.0f));
        Gizmos.DrawLine(new Vector3(globalMaxX, globalMinY, 0.0f), new Vector3(globalMaxX, globalMaxY, 0.0f));
    }
}
