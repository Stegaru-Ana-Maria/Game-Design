using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LaserAttackNode : BTNode
{
    private Transform boss;
    private Transform player;
    private Animator animator;
    private GameObject projectilePrefab;
    private Transform projectileSpawnPoint;
    private float cooldown;
    private float attackDuration;
    private LayerMask playerLayer;
    private float lastAttackTime;
    private float laserAttackRange;
    private IBossAI bossAI;
    // private Boss1AI bossAI;
    private bool isAttacking;

    public LaserAttackNode(Transform boss, Transform player, Animator animator, GameObject projectilePrefab, Transform projectileSpawnPoint, float cooldown, float attackDuration, float laserAttackRange, LayerMask playerLayer)
    {
        this.boss = boss;
        this.player = player;
        this.animator = animator;
        this.projectilePrefab = projectilePrefab;
        this.projectileSpawnPoint = projectileSpawnPoint;
        this.cooldown = cooldown;
        this.attackDuration = attackDuration;
        this.laserAttackRange = laserAttackRange;
        this.playerLayer = playerLayer;
        this.bossAI = boss.GetComponent<IBossAI>();
        this.isAttacking = false;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - lastAttackTime < cooldown)
            return NodeState.FAILURE;

        float distanceToPlayer = Vector2.Distance(boss.position, player.position);
        if (distanceToPlayer > laserAttackRange)
            return NodeState.FAILURE;

        if (!isAttacking)
        {
            bossAI.StartCoroutine(PerformLaserAttack());
            isAttacking = true;
        }

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }

    private IEnumerator PerformLaserAttack()
    {
        animator.SetTrigger("heavyAttack");

        yield return new WaitForSeconds(attackDuration);

        lastAttackTime = Time.time;
        isAttacking = false;

        _nodeState = NodeState.SUCCESS;
    }
}