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
    private Boss1AI bossAI;

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
        bossAI = boss.GetComponent<Boss1AI>();
    }

    public override NodeState Evaluate()
    {
        return NodeState.SUCCESS;
    }

}
