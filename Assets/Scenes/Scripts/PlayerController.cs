using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float moveInput;

    private Rigidbody2D rb;
    private Vector2 v2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //v2 = transform.position;
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed,0);
        //v2 = transform.position;
        //Debug.Log(v2);
    }
}
