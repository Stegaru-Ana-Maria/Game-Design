using UnityEngine;

public class ReturnToPatrolState : DroneState
{
    private Unit pathFollower;
    public ReturnToPatrolState(DroneFSM enemy) : base(enemy)
    {
        pathFollower = enemy.GetComponent<Unit>();
    }
    public override void EnterState()
    {
        Debug.Log("Enter: ReturnToPatroState");
        RequestPathToPatrolPoint();
    }

    public override void UpdateState()
    {
        FlipTowardsPatrolPoint();

        if (Vector2.Distance(enemy.enemy.position, enemy.patrolPointB.position) < 1.5f)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
        Debug.Log(Vector2.Distance(enemy.enemy.position, enemy.patrolPointB.position));
    }

    private void RequestPathToPatrolPoint()
    {
        PathRequestManager.RequestPath(enemy.enemy.position, enemy.patrolPointB.position, pathFollower.OnPathFound);
    }

    private void FlipTowardsPatrolPoint()
    {
        float direction = enemy.patrolPointB.position.x - enemy.enemy.position.x;
        if ((direction < 0 && enemy.enemy.localScale.x > 0) || (direction > 0 && enemy.enemy.localScale.x < 0))
        {
            enemy.enemy.localScale = new Vector3(-enemy.enemy.localScale.x, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit: ReturnToPatroState");
        pathFollower.StopPathFollowing();
    }
}
