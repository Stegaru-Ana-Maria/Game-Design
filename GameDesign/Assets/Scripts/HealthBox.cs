using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public float health =20;
    [SerializeField] private GameObject box;
    [SerializeField] private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(health == 10)
        {
            anim.SetTrigger("hit1");
        }
        
        if (health <= 0)
        {
            anim.SetTrigger("hit2");
            rb.bodyType = RigidbodyType2D.Kinematic;
            box.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public void DestroyBox()
    {
        Destroy(gameObject);
    }
}
