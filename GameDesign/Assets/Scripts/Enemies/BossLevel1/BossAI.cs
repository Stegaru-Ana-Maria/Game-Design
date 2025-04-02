using UnityEngine;
using System.Collections.Generic;

public class BossAI : MonoBehaviour
{
    [SerializeField] private float maxHP = 10f;
    [SerializeField] private float bossHP;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float laserRange = 6f;
    [SerializeField] private float laserCooldownTime = 2f;

    //private float bossHP = 10f;
    private BTNode root;
    private Transform player;
    private bool playerInRoom = false;
    private Animator animator;
    private BossHealth bossHealth;

    void Start()
    {
        bossHealth = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        // ConditionNode: Este Boss-ul sub 30% HP?
        ConditionNode isInFuryMode = new ConditionNode(() => bossHP < maxHP * 0.3f);

        // ConditionNode: Este player-ul in raza de atac laser?
        ConditionNode isPlayerInLaserRange = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < laserRange);

        // ConditionNode: Este player-ul in raza de atac melee?
        ConditionNode isPlayerInMeleeRange = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attackRange);

        // ConditionNode: Este player-ul in camera?
        ConditionNode isPlayerInRoom = new ConditionNode(() => playerInRoom);

        // ActionNode: Ataca cu laser
        ActionNode attackLaser = new ActionNode(() =>
        {
            Debug.Log("Boss ataca cu laser!");
            return NodeState.SUCCESS;
        });

        // ActionNode: Atac melee
        ActionNode attackMelee = new ActionNode(() =>
        {
            Debug.Log("Boss: atac melee!");
            return NodeState.SUCCESS;
        });

        // CooldownNode pentru atac cu laser
        CooldownNode laserCooldown = new CooldownNode(attackLaser, 2f);

        // ActionNode: Mergi spre player
        ActionNode moveToPlayer = new ActionNode(() =>
        {
            Debug.Log("Boss merge spre player!");
            return NodeState.RUNNING;
        });

        // Selector: Alegere atac (melee sau laser)
        Selector chooseAttack = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode> { isPlayerInMeleeRange, attackMelee }),
            new Sequence(new List<BTNode> { isPlayerInLaserRange, laserCooldown })
        });

        ChaseNode chasePlayer = new ChaseNode(transform, player, chaseSpeed, animator);

        // Sequence: Urmarire si atac
        Sequence chaseAndAttack = new Sequence(new List<BTNode>
        {
            isPlayerInRoom,
            new Selector(new List<BTNode>
            {
                new Sequence(new List<BTNode> { isPlayerInLaserRange, chooseAttack }),
                chasePlayer
            })
        });

        // Fury Mode: Ataca doar cu laser
        Sequence furyMode = new Sequence(new List<BTNode>
        {
            isPlayerInLaserRange,
            laserCooldown
        });

        // Root Selector: Fury Mode sau Comportament Normal?
        root = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode> { isInFuryMode, furyMode }),
            chaseAndAttack
        });
    }

    void Update()
    {
        root.Evaluate();
    }

    public float GetHealth()
    {
        return bossHealth.GetHealth();
    }

    public void SetPlayerInRoom(bool isInRoom)
    {
        playerInRoom = isInRoom;
    }

    public void TakeDamage(float damage)
    {
        bossHP -= damage;
        Debug.Log($"Boss HP: {bossHP}/{maxHP}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, laserRange);
    }
}
