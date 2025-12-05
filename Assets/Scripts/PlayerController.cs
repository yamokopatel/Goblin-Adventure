using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpAcceleration;
    private float moveInput;

    private Rigidbody2D rb;
    private Vector2 v2;
    public float direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed,rb.linearVelocity.y);
        if (Input.GetKeyDown("d") /*|| Input.GetKeyDown("right")*/)
        {
            direction = 1;
        }
        else if(Input.GetKeyDown("a") /*|| Input.GetKeyDown("left")*/)
        {
            direction = -1;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rb.linearVelocity = Vector2.up* jumpAcceleration;
        }
    }
    public float GetDirection()
    {
        return direction;
    }
}
