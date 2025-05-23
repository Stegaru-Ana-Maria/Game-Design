using UnityEngine;

public class Boss1Projectile : MonoBehaviour
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
        SoundEffectManager.Play("Laser");
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Shoot ground ...");
            hit = true;
            boxCollider.enabled = false;
            Deactivate();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Shoot player ...");
            hit = true;
            boxCollider.enabled = false;
            Deactivate();
            collision.gameObject.GetComponent<Health>().TakeDamage(1.5f);
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

