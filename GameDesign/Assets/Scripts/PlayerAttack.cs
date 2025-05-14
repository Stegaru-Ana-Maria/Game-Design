using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject[] projectiles;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    public bool rangedAttackUnlocked = false;

    [Header("ENERGY INFORMATION")]
    public int maxEnergy;
    public int currentEnergy;
    [SerializeField] int energyRegenerationAmount = 1;
    public float energyRegenerationTimer = 0;
    public float energyTickerTimer = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        maxEnergy = projectiles.Count() * 10;
        currentEnergy = maxEnergy;
        rangedAttackUnlocked = GameSession.rangedAttackUnlocked;
    }

    private void Update()
    {
        if (rangedAttackUnlocked && Input.GetKeyDown(KeyCode.C) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            HandleAttack();

        cooldownTimer += Time.deltaTime;
        RegenerateEnergy();
    }

    private void HandleAttack()
    {
        if (currentEnergy > 9)
        {
            anim.SetTrigger("shoot");

            Debug.Log("Shooting laser...");
        }
        else
        {
            Debug.Log("NOT ENOUGH AMMUNITION TO SHOOT ! PLEASE WAIT !");
        }

    }

    private void FireProjectile()
    {
        cooldownTimer = 0;
        projectiles[FindBullet()].transform.position = FirePoint.position;
        projectiles[FindBullet()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        currentEnergy -= 10;

    }
    public virtual void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            energyTickerTimer += Time.deltaTime;

            if (energyTickerTimer >= 1)
            {
                energyTickerTimer = 0;
                currentEnergy += energyRegenerationAmount;
            }
        }
    }

    private int FindBullet()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}