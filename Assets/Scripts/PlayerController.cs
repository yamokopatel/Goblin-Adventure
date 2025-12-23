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
    private float currentY;
    private float previousY;
    private float direction;
    private float previousDirection;

    private bool isGrounded;
    private bool isHanging;
    private bool isClimbing;
    public Transform feetPos;
    public Transform hangPos;
    public Transform handPos;
    public Transform climbPos;
    public float checkRaduis;
    public LayerMask whatIsGround;
    public LayerMask whatIsClimbable;

    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        direction = 1;
        currentX = transform.position.x;
        currentY = transform.position.y;
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
        CheckGrounded();
        if(isGrounded == false)
        {
            ChechHanging();
        }
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
        if (!CheckHand())
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
    }
    private void Jump()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rb.linearVelocity = Vector2.up * jumpAcceleration;
        }
        /*else if(Input.GetKeyDown("space") && !isGrounded && isHanging)
        {
            rb.linearVelocity = Vector2.up * jumpAcceleration / 2;
            rb.linearVelocity = Vector2.right * direction * speed / 2;
        }
        else if(Input.GetKeyDown("space") && isGrounded && isHanging)
        {
            rb.linearVelocity = Vector2.up * jumpAcceleration;
            rb.linearVelocity = Vector2.right * direction * speed / 4;
        }*/
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
        if(direction != previousDirection && !isHanging)
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1f;
            transform.localScale = scaler;
        }
        ChechHanging();
    }
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRaduis, whatIsGround);
    }
    private void ChechHanging()
    {
        /*previousY = currentY;
        currentY = transform.position.y;
        float dY = currentY - previousY;
        if(dY < 0f)
        {
            dY *= -1f;
        }
        float dX = currentX - previousX;
        if (dX < 0f)
        {
            dX *= -1f;
        }

        if (!isGrounded && dY < 0.1f && dX < 0.3f)
        {
            isHanging = true;
        }
        else
        {
            isHanging = false;
        }*/
        isHanging = Physics2D.OverlapCircle(hangPos.position, checkRaduis, whatIsGround) && !isGrounded;
    }
    private bool CheckHand()
    {
        if (isGrounded)
        {
            return Physics2D.OverlapCircle(handPos.position, checkRaduis, whatIsGround);
        }
        return false;
    }
}
