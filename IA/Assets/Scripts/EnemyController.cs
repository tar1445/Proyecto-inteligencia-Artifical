using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight los;
    [SerializeField] private FSM fsm;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isNear;

    // Variables para el wander
    private Vector3 wanderDirection;
    private float wanderTimer;

    [SerializeField] private float wanderChangeInterval = 2f;
    [SerializeField] private float wanderTurnSpeed = 45f;

    [SerializeField] private float checkDistance = 2f;
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        if (los == null)
        {
            los = GetComponent<LineOfSight>();
        }

        if (fsm == null)
        {
            fsm = GetComponent<FSM>();
        }
    }

    private void Start()
    {
        wanderDirection = transform.forward;
    }
    void Update()
    {
        bool canSeePlayer = los.CheckRange(transform, player.transform)
            && los.CheckAngle(transform, player.transform)
            && los.checkObstacles(transform, player.transform);

        fsm.UpdateState(canSeePlayer, isNear);

        ExecuteState();
    }

    void ExecuteState()
    {
        switch (fsm.currentState)
        {
            case FSM.EnemyState.Patrol:
                _Patrol();
                break;

            case FSM.EnemyState.Persuit:

                _Pursuit();
                break;

            case FSM.EnemyState.Kill:

                _kill();
                break;
            case FSM.EnemyState.Wander:
                _Wander();
                break;
        }


    }

    void _Patrol()
    {
        transform.Rotate(Vector3.up * 30f * Time.deltaTime);
    }

    void _Pursuit()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        Vector3 moveDir = dir.normalized;

        transform.position += moveDir * speed * Time.deltaTime;

        transform.forward = Vector3.Lerp(
            transform.forward,
            moveDir,
            Time.deltaTime * rotationSpeed
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNear = true;
            Debug.Log("colission");
        }
    }
    private void OnCollisionExit(Collision collision) { isNear = false; }

    void _kill()
    {
        ResetScene();
        Debug.Log("Game Over");
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void _Wander()
    {
        Vector3 dir = Wander();

        // 🔍 Chequear si hay suelo adelante
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 forwardCheck = origin + dir * checkDistance;

        RaycastHit hit;

        // Tira un rayo hacia abajo desde adelante
        bool haySuelo = Physics.Raycast(forwardCheck, Vector3.down, out hit, groundCheckDistance, groundLayer);

        if (!haySuelo)
        {
            // ❌ No hay suelo → girar
            wanderDirection = Quaternion.Euler(0, Random.Range(90, 180), 0) * wanderDirection;
            return;
        }

        // ✔️ Hay suelo → moverse normal
        transform.position += dir * speed * Time.deltaTime;

        transform.forward = Vector3.Lerp(
            transform.forward,
            dir,
            Time.deltaTime * rotationSpeed
        );
    }

    private Vector3 Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            float randomAngle = Random.Range(-wanderTurnSpeed, wanderTurnSpeed);

            Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);

            wanderDirection = rotation * wanderDirection;
            wanderDirection.y = 0f;
            wanderDirection.Normalize();

            wanderTimer = wanderChangeInterval;
        }

        return wanderDirection;
    }
}
