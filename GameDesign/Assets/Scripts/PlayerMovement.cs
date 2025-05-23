using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] float speed;
    [SerializeField] float jumpingPower;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private BoxCollider2D boxCollider;
    private Animator anim;
    public GameObject attackPoint;
    public Vector2 boxSize;
    public LayerMask enemies;
    public float punchingDamage;
    public float fallGravity;
    public float gravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.X) && IsGrounded())
        {
            anim.SetTrigger("isPunching");
            SoundEffectManager.Play("PlayerAttack");
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            anim.SetBool("Jump",true);
            SoundEffectManager.Play("Jump");
        }
        
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            rb.gravityScale = fallGravity;
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
        }
        if (IsGrounded() == false && rb.linearVelocity.y < 0f && anim.GetBool("onLadder") == false)
        {
            rb.gravityScale = fallGravity;
            anim.SetBool("Fall", true);
        }
        if (IsGrounded())
        {
            rb.gravityScale = gravity;
            anim.SetBool("Fall", false);
        }


            Flip();

        anim.SetBool("run", horizontal != 0);
        anim.SetBool("grounded", IsGrounded());

    }

    public void endPunching()
    {
        anim.ResetTrigger("isPunching");
    }

    public void punch()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(attackPoint.transform.position, boxSize, 0, enemies);

        foreach (Collider2D enemyGameObject in enemy)
        {
            if (enemyGameObject.CompareTag("Enemy"))
            {
                Debug.Log("You hit an enemy...");
                enemyGameObject.GetComponent<EnemyHealth>().TakeDamage(1);

            }
            else if (enemyGameObject.CompareTag("Box"))
            {
                Debug.Log("You hit a box...");
                enemyGameObject.GetComponent<HealthBox>().health -= punchingDamage;
            }
            else if (enemyGameObject.CompareTag("Boss"))
            {
                Debug.Log("You hit the boss...");
                enemyGameObject.GetComponent<BossHealth>().TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.transform.position, boxSize);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }
    

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
        

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public bool canAttack()
    {
        return horizontal == 0;
    }
}
