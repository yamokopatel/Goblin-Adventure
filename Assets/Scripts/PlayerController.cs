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
    private bool isStanding;
    private bool isIdle;
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
        CheckIdle();
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
        UpdateAnim();
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
        //to find where throw spear
        float xAddentum;
        if (CheckPos(spearSpawnPos, whatIsGround, true))
        {
            xAddentum = direction / 2;
        }
        else
        {
            xAddentum = direction;
        }
        //without that spear can be launched OBB
        //OBB - Out Of Bounds
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
    /*private void CheckStanding()
    {
        if(currentX == previousX)
        {
            if(standingTime < 301f)
            {
                standingTime++;
            }
        }
        else
        {
            standingTime = 0;
        }
    }*/
    private void Climb()
    {
        if (ableToClimbing)
        {
            if(GetVertical() != 0)
            {
                isClimbing = true;
            }
            if(isClimbing)
            {
                if(GetVertical() != 0)
                {
                    verInput = GetVertical();
                    rb.linearVelocity = new Vector2(moveInput * speed / 2.5f, verInput * speed);

                }
                else
                {
                    //to goblin don't slide while climbing
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0.1962f);
                }
            }
        }
        else
        {
            isClimbing = false;
            //to can't start jump in air
        }
    }
    private void UpdateAnim()
    {
        anim.SetBool("isOnGround", isGrounded);
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("isHanging", isHanging);
        anim.SetBool("isStanding", isStanding);
        anim.SetBool("isIdle", isIdle);
    }
    private void CheckStanding()
    {
        if(isGrounded && previousX == currentX)
        {
            isStanding = true;
        }
        else
        {
            isStanding = false;
        }
    }
    private void CheckIdle()
    {
        if (isGrounded && !isIdle)
        {
            if(isStanding)
            {
                standingTime++;
                if (standingTime > 300)
                {
                    isIdle = true;
                }
            }
        }
        if(isIdle && (!isStanding || isClimbing || !isGrounded))
        {
            isIdle = false;
        }
    }


    // DRY functions
    private bool CheckPos(Transform pos, LayerMask checkLayer, bool adjunctBool)
    {
        return Physics2D.OverlapCircle(pos.position, checkRaduis, checkLayer) && adjunctBool;
    }
    private float GetVertical()
    {
        return Input.GetAxis("Vertical");
    }
}
