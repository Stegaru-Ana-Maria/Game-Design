using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class VentMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 7f;
    private bool isLadder;
    private bool onLadder;
    private Animator anim;

    [SerializeField] private Rigidbody2D rb;
    private float gravity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            onLadder = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }
        if (Input.GetButtonDown("Jump") && onLadder == true)
        {
            onLadder = false;
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
        anim.SetBool("onLadder",onLadder);
    }

    private void FixedUpdate()
    {
        if (rb.gravityScale != 0f) 
        {
            gravity = rb.gravityScale;
        }
        if (onLadder)
        {
            anim.SetInteger("climbing", (int)(rb.linearVelocity.y + vertical * speed)); 
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
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
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            onLadder = false;
            anim.SetBool("Fall", true);
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; ;
        }
    }
}