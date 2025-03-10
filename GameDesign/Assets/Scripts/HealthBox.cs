using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public float health =20;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

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
            Debug.Log("Enemy is dead");

        }
    }

    public void DestroyBox()
    {
        Destroy(gameObject);
    }
}
