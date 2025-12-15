using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpAcceleration;
    private float moveInput;
    public GameObject spearPrefab;
    private float spearCooldownTime;

    private Rigidbody2D rb;
    private float currentX;
    private float previousX;
    private float direction;
    private float previousDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        currentX = transform.position.x;
        spearCooldownTime = 0;
    }

    private void FixedUpdate()
    {
        Walk();
        GetDirection();
        Flip();
        SpearThrow();
        SpearCooldown();
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
        previousDirection = direction;
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
    private void SpearThrow()
    {
        if (Input.GetKeyDown("e") && spearCooldownTime == 0f)
        {
            Vector2 launchPos = new Vector2(transform.position.x + direction, transform.position.y);
            GameObject spearInstance = Instantiate(spearPrefab,launchPos,Quaternion.identity);
            spearCooldownTime = 150f;
        }
    }
    private void SpearCooldown()
    {
        if(spearCooldownTime > 0f)
        {
            spearCooldownTime--;
        }
    }
    private void Flip()
    {
        if(direction != previousDirection)
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1f;
            transform.localScale = scaler;
        }
    }
}
