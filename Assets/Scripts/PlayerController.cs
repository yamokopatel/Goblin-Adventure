using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpAcceleration;
    private float moveInput;
    private float verInput;
    public GameObject spearPrefab;
    public float secSpearCooldown;
    public Transform spearSpawnPos;
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
    private bool ableToClimbing;
    private bool isClimbing;
    public Transform feetPos;
    public Transform hangPos;
    public Transform handPos;
    public Transform climbPos;
    public float checkRaduis;
    public LayerMask whatIsGround;
    public LayerMask whatIsClimbable;

    private Animator anim;
    private float standingTime;

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
        Climb();
        GetDirection();
        Flip();
        SpearThrow();
        SpearCooldown();
        CheckStanding();
    }
    private void Update()
    {
        isGrounded = CheckPos(feetPos,whatIsGround,true);
        ableToClimbing = CheckPos(climbPos,whatIsClimbable, true);
        if(isGrounded == false)
        {
            isHanging = CheckPos(hangPos,whatIsGround,!isGrounded);
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
        if (!CheckPos(handPos,whatIsGround,isGrounded))
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
        if (Input.GetKeyDown("space") && (isGrounded || isClimbing))
        {
            rb.linearVelocity = Vector2.up * jumpAcceleration;
            if (isClimbing)
            {
                isClimbing = false;
            }
        }
    }
    private void SpearThrow()
    {
        float xAddentum;
        if (CheckPos(spearSpawnPos, whatIsGround, true))
        {
            xAddentum = direction / 2;
        }
        else
        {
            xAddentum = direction;
        }
        //
        if (Input.GetKeyDown("e") && spearCooldownTime == 0f)
        {
            Vector2 launchPos = new Vector2(transform.position.x + xAddentum, transform.position.y);
            GameObject spearInstance = Instantiate(spearPrefab,launchPos,Quaternion.identity);
            spearCooldownTime = secSpearCooldown * 50f;
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
        isHanging = CheckPos(hangPos, whatIsGround, !isGrounded);
    }
    private bool CheckPos(Transform pos, LayerMask checkLayer, bool adjunctBool)
    {
        return Physics2D.OverlapCircle(pos.position, checkRaduis, checkLayer) && adjunctBool;
    }
    private void CheckStanding()
    {
        if(currentX != previousX)
        {
            standingTime++;
            if(standingTime > 301)
            {
                standingTime--;
            }
        }
        else
        {
            standingTime = 0;
        }
    }
    private void Climb()
    {
        /*if (isClimbing)
        {

            verInput = Input.GetAxis("Vertical");
            rb.linearVelocity = new Vector2(moveInput * speed / 2.5, verInput * speed);
        }*/
        if (ableToClimbing)
        {
            if(Input.GetAxis("Vertical") != 0)
            {
                isClimbing = true;
            }
            if(isClimbing)
            {
                if(Input.GetAxis("Vertical") != 0)
                {
                    verInput = Input.GetAxis("Vertical");
                    rb.linearVelocity = new Vector2(moveInput * speed / 2.5f, verInput * speed);

                }
                else
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0.195f);
                }
            }
        }
        else
        {
            isClimbing = false;
        }
    }
}
