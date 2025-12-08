using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpAcceleration;
    private float moveInput;
    public float direction;

    private Rigidbody2D rb;
    private float currentX;
    private float previousX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        currentX = transform.position.x;
    }

    private void FixedUpdate()
    {
        Walk();
        GetDirection();
    }
    private void Update()
    {
        Jump();
    }
    //Action functions
    private void Walk()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
    private void GetDirection()
    {
        previousX = currentX;
        currentX = transform.position.x;
        if (currentX > previousX)
        {
            direction = 1;
        }
        else if (currentX < previousX)
        {
            direction = -1;
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown("space"))
        {
            rb.linearVelocity = Vector2.up * jumpAcceleration;
        }
    }
}
