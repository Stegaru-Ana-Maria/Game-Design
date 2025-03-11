using UnityEngine;

public class VentMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 7f;
    private bool isVent;
    private bool isAscending;
    
   [SerializeField] private Rigidbody2D rb;
    private float gravity;

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isVent && Mathf.Abs(vertical) > 0f)
        {
            isAscending = true;
            
        }
    }

    private void FixedUpdate()
    {
        if (rb.gravityScale != 0f) 
        {
            gravity = rb.gravityScale;
        }
        if (isAscending)
        {
            
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vent"))
        {
            isVent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Vent"))
        {
            isVent = false;
            isAscending = false;
        }
    }
}