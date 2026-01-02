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

    private bool ableToClimbing;
    public Transform feetPos;
    public Transform hangPos;
    public Transform handPos;
    public Transform climbPos;
    public float checkRaduis;
    public LayerMask whatIsGround;
    public LayerMask whatIsClimbable;

    private bool[/* isGrounded, isClimbing, isHanging, isStanding, isIdle */] states;

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
        states = new bool[5];
    }

    private void FixedUpdate()
    {
        CheckClimbing();
        CheckStanding();
        CheckIdle();
        Walk();
        Climb();
        GetDirection();
        Flip();
        SpearThrow();
        SpearCooldown();
        UpdateAnim();
    }
    private void Update()
    {
        states[0] = CheckPos(feetPos,whatIsGround,true);
        ableToClimbing = CheckPos(climbPos,whatIsClimbable, true);
        if (states[0] == false)
        {
            states[2] = CheckPos(hangPos, whatIsGround, !states[0]);
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
        if (!CheckPos(handPos, whatIsGround, states[0]))
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
        if (Input.GetKeyDown("space") && (states[0] || states[1]))
        {
            rb.linearVelocity = Vector2.up * jumpAcceleration;
            if (states[1])
            {
                states[1] = false;
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
        if(direction != previousDirection && !states[2])
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1f;
            transform.localScale = scaler;
        }
        states[2] = CheckPos(hangPos, whatIsGround, !states[0]);
    }
    private void CheckClimbing()
    {
        if (ableToClimbing)
        {
            if (GetVertical() != 0)
            {
                states[1] = true;
            }
        }
        if(!ableToClimbing && states[1])
        {
            states[1] = false;
            //to can't start jump in air
        }
    }
    private void Climb()
    {
        if (states[1])
        {
            if (GetVertical() != 0)
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
    private void UpdateAnim()
    {
        anim.SetBool("isOnGround", states[0]);
        anim.SetBool("isClimbing", states[1]);
        anim.SetBool("isHanging", states[2]);
        anim.SetBool("isStanding", states[3]);
        anim.SetBool("isIdle", states[4]);
    }
    private void CheckStanding()
    {
        if (states[0] && previousX == currentX)
        {
            states[3] = true;
        }
        else
        {
            states[3] = false;
        }
    }
    private void CheckIdle()
    {
        if (states[0] && !states[4])
        {
            if(states[3])
            {
                standingTime++;
                if (standingTime > 300)
                {
                    states[4] = true;
                }
            }
        }
        if(states[4] && (!states[3] || states[1] || !states[0]))
        {
            states[4] = false;
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
