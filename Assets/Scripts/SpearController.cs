using UnityEngine;

public class SpearController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float existTime;
    public float hSpeed;
    private float direction;

    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        existTime = 3000f;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        float playerX = player.transform.position.x;
        if (playerX < transform.position.x)
        {
            direction = 1f;
        }
        else if (playerX > transform.position.x)
        {
            direction = -1f;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1f;
            transform.localScale = scaler;
        }
        
    }

    private void FixedUpdate()
    {
        if(existTime > 0)
        {
            rb.linearVelocity = Vector2.right * direction * hSpeed;
            existTime--;
        }
        else
        {
            rb.linearVelocity = Vector2.up * existTime;
            if(existTime < -5)
            {
                Destroy(this.gameObject);
            }
            existTime -= 0.2f;
        }
        
    }
}
