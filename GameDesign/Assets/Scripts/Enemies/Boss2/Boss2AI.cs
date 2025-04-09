using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Boss2AI : MonoBehaviour, IBossAI
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 10f;
    [SerializeField] public float currentHP;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private SpriteRenderer bossSprite;
    [SerializeField] private Color rageModeColor = Color.red;

    [Header("Ranges")]
    [SerializeField] private float attack1Range = 3f;
    [SerializeField] private float attack2Range = 3f;
    [SerializeField] private float laserAttackRange = 6f;

    [Header("Cooldowns")]
    [SerializeField] private float attackCooldownTime = 2f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float rageCooldownTime = 1f;

    [Header("Dependencies")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attack1Duration = 0.5f;
    [SerializeField] private float attack2Duration = 0.6f;

    [Header("Special Attack - Laser")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private GameObject healEffectPrefab;

    private BTNode root;
    private Transform player;
    private Animator animator;
    private BossHealth bossHealth;
    private Rigidbody2D rb;

    private bool playerInRoom = false;
    public bool isPerformingLaserAttack = false;
    private bool shieldActive = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        #region Condition Nodes
        var isPlayerInRoom = new ConditionNode(() => playerInRoom);
        var isPlayerInAttack1Range = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack1Range);
        var isPlayerInAttack2Range = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack2Range);
        var isPlayerInLaserAttackRange = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < laserAttackRange);
        var isTooFarForAttack = new ConditionNode(() => Vector2.Distance(transform.position, player.position) > attack1Range);
        var isTooFarForLaserAttack = new ConditionNode(() => Vector2.Distance(transform.position, player.position) > laserAttackRange);
        var isLowHP = new ConditionNode(() => bossHealth.GetHealth() < maxHP * 0.3f);
        var isRageMode = new ConditionNode(() => bossHealth.GetHealth() <= maxHP * 0.15f);

        var isInDefensiveMode = new ConditionNode(() =>
        {
            float hp = bossHealth.GetHealth();
            return hp <= maxHP * 0.3f && hp > maxHP * 0.15f;
        });
        #endregion

        #region Actions
        var attack1 = new LightAttackNode(
            transform, player, animator,
            "attack1",
            cooldown: 2f,
            attackDuration: attack1Duration,
            damage: 1f,
            knockbackForce: 5f,
            playerLayer: playerLayer,
            attackRange: attack1Range,
            damageFrameRatio: 5f / 6f
        );

        var attack2 = new LightAttackNode(
            transform, player, animator,
            "attack2",
            cooldown: 3f,
            attackDuration: attack2Duration,
            damage: 1.5f,
            knockbackForce: 7f,
            playerLayer: playerLayer,
            attackRange: attack2Range,
            damageFrameRatio: 5f / 6f
        );

        var laserAttack = new LaserAttackNode(
            transform, player, animator, projectilePrefab, projectileSpawnPoint,
            cooldown: 2f,
            attackDuration: 2.0f,
            laserAttackRange: laserAttackRange,
            playerLayer: LayerMask.GetMask("Player")
        );

        var activateShield = new ActionNode(() =>
        {
            if (!shieldActive)
            {
                shieldObject.SetActive(true);
                shieldActive = true;
                Debug.Log("Shield activated.");
            }
            return NodeState.SUCCESS;
        });

        var deactivateShield = new ActionNode(() =>
        {
            if (shieldObject != null && shieldObject.activeSelf)
            {
                shieldObject.SetActive(false);
                shieldActive = false;
                Debug.Log("Shield deactivated.");
            }
            //
            return NodeState.SUCCESS;
        });

        var heal = new ActionNode(() =>
        {
            bossHealth.Heal(bossHealth.GetMaxHealth() * 0.1f);
            Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("Boss healed.");
            return NodeState.SUCCESS;
        });

        var chase = new ChaseNode(transform, player, chaseSpeed, animator, rb, obstacleMask);

        var setBerserkSpeed = new ActionNode(() =>
        {
            chase.SetSpeed(chaseSpeed * 2f);
            return NodeState.SUCCESS;
        });

        var enterRageMode = new ActionNode(() =>
        {
            if (bossSprite != null)
                bossSprite.color = rageModeColor;
            Debug.Log("Entered Rage Mode!");
            return NodeState.SUCCESS;
        });

        var idle = new ActionNode(() =>
        {
            animator.SetBool("isRunning", false);
            return NodeState.SUCCESS;
        });
        #endregion

        #region Cooldowns
        var projectileCooldown = new CooldownNode(laserAttack, attackCooldownTime);
        var healCooldownNode = new CooldownNode(heal, healCooldown);
        var rageAttack3 = new CooldownNode(laserAttack, rageCooldownTime);
        var rageAttack1 = new CooldownNode(attack1, rageCooldownTime);
        var rageAttack2 = new CooldownNode(attack2, rageCooldownTime);
        #endregion

        var approachAndAttack = new Sequence(new List<BTNode>
        {
            new Repeater(
                new Sequence(new List<BTNode>
                {
                    new ConditionNode(() => Vector2.Distance(transform.position, player.position) > laserAttackRange),
                    chase
                }),
                repeatUntilSuccess: true
                ),
            projectileCooldown
        });

        #region Subtrees

        // --- Rage ---

        var rageTree = new Sequence(new List<BTNode>
        {
            isRageMode,
            enterRageMode,
            setBerserkSpeed,
            new Repeater(
            new Selector(new List<BTNode>
            {
                new Sequence(new List<BTNode>
                {
                    isPlayerInAttack2Range,
                    rageAttack2
                }),
                new Sequence(new List<BTNode>
                {
                    isPlayerInAttack1Range,
                    rageAttack1
                }),
                new Sequence(new List<BTNode>
                {
                    isPlayerInLaserAttackRange,
                    rageAttack3
                }),
                chase
            }),
            repeatUntilSuccess: false
            )
        });

        // --- Defensive ---
        var defensiveTree = new Sequence(new List<BTNode>
        {
            isInDefensiveMode,
            activateShield,
            approachAndAttack,
            deactivateShield,
            healCooldownNode
        });


        var combatTree = new Sequence(new List<BTNode>
        {
            isPlayerInRoom,
            new Selector(new List<BTNode>
            {
                new Sequence(new List<BTNode> { isPlayerInAttack1Range, attack1 }),
                new Sequence(new List<BTNode> { isPlayerInAttack2Range, attack2 }),
                new Sequence(new List<BTNode> { isPlayerInLaserAttackRange, projectileCooldown }),
                new Sequence(new List<BTNode> { isTooFarForAttack, chase })
            })
        });

        #endregion

        // --- Final Root Tree ---
        root = new Selector(new List<BTNode>
        {
            rageTree,
            defensiveTree,
            combatTree,
            idle
        });
    }

    void Update()
    {
        root?.Evaluate();

        currentHP = bossHealth.GetHealth();
    }

    public float GetHealth()
    {
        return bossHealth.GetHealth();
    }

    public void SetPlayerInRoom(bool isInRoom)
    {
        playerInRoom = isInRoom;
    }

    public void SpawnProjectile()
    {
        GameObject proj = ObjectPool.Instance.GetFromPool(projectilePrefab);
        if (proj != null)
        {
            proj.transform.position = projectileSpawnPoint.position;
            float direction = Mathf.Sign(transform.localScale.x);
            proj.GetComponent<Boss1Projectile>().SetDirection(direction);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attack1Range);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attack2Range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, laserAttackRange);
    }
}
