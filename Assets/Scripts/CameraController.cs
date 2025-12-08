using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        Vector2 playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 0.5f, -10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 selfPosition = transform.position;
        Vector2 playerPosition = player.transform.position;
        //deltas
        float dX = playerPosition.x - selfPosition.x;
        float dY = playerPosition.y - selfPosition.y + 0.5f;
        //distance
        float distance = Mathf.Sqrt(dX * dX + dY * dY);
        //move
        float newX;
        float newY;
        if (distance < 0.1)
        {
            newX = (float)(dX * 0.1);
            newY = (float)(dY * 0.1);
        }
        else if (distance < 0.3)
        {
            newX = (float)(dX * 0.2);
            newY = (float)(dY * 0.2);
        }
        else if (distance < 0.6)
        {
            newX = (float)(dX * 0.5);
            newY = (float)(dY * 0.5);
        }
        else if (distance < 0.8)
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
