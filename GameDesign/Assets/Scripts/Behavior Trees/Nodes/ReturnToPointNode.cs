using UnityEngine;

public class ReturnToPointNode : BTNode
{
    private Transform bossTransform;
    private Transform startPoint;
    private float speed;
    private Animator animator;
    private Rigidbody2D rb;
    private LayerMask obstacleMask;
    private Boss1AI bossAI;

    private float stopDistance = 0.5f;
    private float jumpForce = 8f;
    private float obstacleCheckDistance = 2.0f;
    private float groundCheckDistance = 1.5f;

    public ReturnToPointNode(Transform bossTransform, Transform startPoint, float speed, Animator animator, Rigidbody2D rb, LayerMask obstacleMask, Boss1AI bossAI)
    {
        this.bossTransform = bossTransform;
        this.startPoint = startPoint;
        this.speed = speed;
        this.animator = animator;
        this.rb = rb;
        this.obstacleMask = obstacleMask;
        this.bossAI = bossAI;
    }

    public override NodeState Evaluate()
    {
        if (startPoint == null)
        {
            animator.SetBool("isRunning", false);
            return NodeState.FAILURE;
        }

        float distance = Vector2.Distance(bossTransform.position, startPoint.position);
        if (distance <= stopDistance)
        {
            animator.SetBool("isRunning", false);
            bossAI.SetReturningToPoint(false);
            return NodeState.SUCCESS;
        }

        animator.SetBool("isRunning", true);
        FlipTowardsPoint();

        if (IsObstacleAhead() && IsGrounded())
        {
            Jump();
        }

        bossTransform.position = Vector2.MoveTowards(
            bossTransform.position,
            new Vector2(startPoint.position.x, bossTransform.position.y),
            speed * Time.deltaTime
        );

        return NodeState.RUNNING;
    }

    private void FlipTowardsPoint()
    {
        if ((startPoint.position.x < bossTransform.position.x && bossTransform.localScale.x > 0) ||
            (startPoint.position.x > bossTransform.position.x && bossTransform.localScale.x < 0))
        {
            bossTransform.localScale = new Vector3(-bossTransform.localScale.x, bossTransform.localScale.y, bossTransform.localScale.z);
        }
    }

    private bool IsObstacleAhead()
    {
        Vector2 direction = bossTransform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(bossTransform.position.x, bossTransform.position.y + 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, obstacleCheckDistance, obstacleMask);

        Debug.DrawRay(origin, direction * obstacleCheckDistance, Color.blue);

        return hit.collider != null;
    }

    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(bossTransform.position.x, bossTransform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, obstacleMask);

        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.green);

        return hit.collider != null;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("jump");
    }
}
