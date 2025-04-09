using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 2) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Shoot enemy ...");
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");
            collision.GetComponent<EnemyHealth>().TakeDamage(1);

        }else  if (collision.CompareTag("Ground"))
        {
            Debug.Log("Shoot ground ...");
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");

        }else if (collision.CompareTag("Box"))
        {
            Debug.Log("Shoot box ...");
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");
            collision.GetComponent<HealthBox>().health -= 10;

        }else if (collision is BoxCollider2D && collision.CompareTag("Door"))
        {
            Debug.Log("Hit the BoxCollider2D of the door.");
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");
        }else if (collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("Shoot boss ...");
            hit = true;
            boxCollider.enabled = false;
            anim.SetTrigger("explode");
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(2.5f);
        }

    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
