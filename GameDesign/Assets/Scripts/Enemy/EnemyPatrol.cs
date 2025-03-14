using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform PatrolPoint0;
    [SerializeField] private Transform PatrolPoint1;

    [SerializeField] private Transform enemy;

    [SerializeField] private float speed;
    [SerializeField] private float chasingSpeed;
    private Vector3 initScale;
    private bool movingLeft;

    [SerializeField] private float idleDuration;
    private float idleTimer;

    [SerializeField] private Animator anim;

    [SerializeField]  private Transform playerTransform;
    public float chaseDistance;
    public float stopChaseDistance;
    private bool isChasing = false;

    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform nu este setat!");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        //Debug.Log("Distanta pana la player: " + distanceToPlayer);

        if (isChasing)
        {
            if (distanceToPlayer > stopChaseDistance) 
            {
                Debug.Log("Playerul a scapat, revenim la patrulare");
                isChasing = false;
            }
            else
            {
                Debug.Log("Urmarim player-ul!");
                ChasePlayer();
                return; 
            }
        }
        else
        {
            if (distanceToPlayer < chaseDistance) 
            {
                Debug.Log("Playerul a intrat in raza de urmarire!");
                isChasing = true;
                return; 
            }

            Patrol();
        }
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= PatrolPoint0.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= PatrolPoint1.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void ChasePlayer()
    {
        anim.SetBool("moving", true);

        int direction = (playerTransform.position.x > enemy.position.x) ? 1 : -1; 

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * chasingSpeed,
            enemy.position.y, enemy.position.z);
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0;
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }

}
