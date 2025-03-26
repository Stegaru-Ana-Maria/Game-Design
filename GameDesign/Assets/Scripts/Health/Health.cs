using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Health : MonoBehaviour
{
    public GameObject gameObj;
    public Transform respawPoint;
    private Rigidbody2D rb;
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private bool hurt;

    public Material damagedMaterial;
    private float hurtTimer;
    private float damagedTime = (float) 0.1;
    
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hurtTimer = damagedTime;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        damagedMaterial.SetFloat("_ColorMask", (float)9);
        hurt = true;

        if (currentHealth <=0)
        {
            if (!dead)
            {
                anim.SetTrigger("grounded");
                anim.SetTrigger("die");

                if (GetComponent<PlayerMovement>() != null)
                {
                    GetComponent<PlayerMovement>().enabled = false;
                }

                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                

                dead = true;
            }
        }
    }
    private void Update()
    {
        if (hurt == true)
        {
            if (hurtTimer > 0)
            {
                hurtTimer -= Time.deltaTime;
            }
            else
            {
                hurt = false;
                hurtTimer = damagedTime;
                damagedMaterial.SetFloat("_ColorMask", (float)14);
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private void RespawnEvent() // pt player
    {
        dead = false;
        gameObj.transform.position = respawPoint.position;

        GetComponent<PlayerMovement>().enabled = true;
        rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        currentHealth = startingHealth;
    }

    private void Deactivate() // pt inamici
    {
        gameObject.SetActive(false);
    }
}