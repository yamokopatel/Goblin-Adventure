using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 selfPosition = transform.position;
        Vector2 playerPosition = GameObject.Find("Player").transform.position;
        //deltas
        float dX = playerPosition.x - selfPosition.x;
        float dY = playerPosition.y - selfPosition.y + 0.5f;
        //distance
        float distance = Mathf.Sqrt(dX * dX + dY * dY);
        //move
        float newX;
        float newY;
        if (distance <= 8)
        {
            newX = (float)(dX * 0.8);
            newY = (float)(dY * 0.8);
        }
        else
        {
            newX = (float)(dX * 0.9);
            newY = (float)(dY * 0.9);
        }
        transform.position = new Vector3(selfPosition.x + newX, selfPosition.y + newY, -10);
    }
}
