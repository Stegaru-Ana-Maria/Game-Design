using UnityEngine;

public class ChaseNode : BTNode
{
    private Transform bossTransform;
    private Transform playerTransform;
    private float speed;
    private Animator animator;

    public ChaseNode(Transform bossTransform, Transform playerTransform, float speed, Animator animator)
    {
        this.bossTransform = bossTransform;
        this.playerTransform = playerTransform;
        this.speed = speed;
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (playerTransform == null)
        {
            animator.SetBool("isRunning", false);
            return NodeState.FAILURE; 
        }

        animator.SetBool("isRunning", true);

        FlipTowardsPlayer();

        bossTransform.position = Vector2.MoveTowards(
            bossTransform.position,
            playerTransform.position,
            speed * Time.deltaTime
        );

        return NodeState.RUNNING; 
    }

    private void FlipTowardsPlayer()
    {
        if ((playerTransform.position.x < bossTransform.position.x && bossTransform.localScale.x > 0) ||
            (playerTransform.position.x > bossTransform.position.x && bossTransform.localScale.x < 0))
        {
            bossTransform.localScale = new Vector3(-bossTransform.localScale.x, bossTransform.localScale.y, bossTransform.localScale.z);
        }
    }
}
