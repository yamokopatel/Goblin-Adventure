using UnityEngine;

public class SpearController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float existTime;
    public float hSpeed;
    private float direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        existTime = 3000f;
        rb = GetComponent<Rigidbody2D>();
        direction = System.Convert.ToSingle(GameObject.Find("Player").GetComponent("direction"));
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
