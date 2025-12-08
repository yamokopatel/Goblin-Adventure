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
        float q;
        if (distance < 0.1f)
        {
            q = 0.1f;
        }
        else if (distance < 0.3f)
        {
            q = 0.2f;
        }
        else if (distance < 0.6f)
        {
            q = 0.5f;
        }
        else if (distance < 0.8f)
        {
            q = 0.8f;
        }
        else
        {
            q = 0.9f;
        }
        newX = dX * q;
        newY = dY * q;
        transform.position = new Vector3(selfPosition.x + newX, selfPosition.y + newY, -10);
    }
}
