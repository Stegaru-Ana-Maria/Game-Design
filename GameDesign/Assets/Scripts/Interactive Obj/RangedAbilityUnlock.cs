using UnityEngine;

public class RangedAbilityUnlock : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerAttack playerAttack = collision.GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.rangedAttackUnlocked = true;
                GameSession.rangedAttackUnlocked = true; 
                Debug.Log("Ranged attack unlocked!");
                Destroy(gameObject);
            }
        }
    }
}
