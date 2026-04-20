using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight los;
    [SerializeField] Rigidbody rb;
    [SerializeField] private FSM fsm;
    [SerializeField] Animator _animation;
    [SerializeField] private float speedNormal;
    [SerializeField] private float PersuitSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float PersuitrotationSpeed;
    [SerializeField] private bool isNear;
   

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float pointReachDistance = 1.5f;

    [SerializeField] private float wallCheckDistance = 1.5f;
    [SerializeField] private LayerMask wallLayer;

    private int currentPoint = 0;
   



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

        rb = GetComponent<Rigidbody>(); 

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
        _animation.SetBool("correr", true);
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        Vector3 moveDir = dir.normalized;

        transform.position += moveDir * PersuitSpeed * Time.deltaTime;

        transform.forward = Vector3.Lerp(
            transform.forward,
            moveDir,
            Time.deltaTime * PersuitrotationSpeed);
       

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
        FindObjectOfType<GameManager>().PlayerDied();
        Debug.Log("Game Over");
    }

 
    void _Wander()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform target = patrolPoints[currentPoint];

        // Dirección hacia el punto
        Vector3 direction = (target.position - transform.position);
        direction.y = 0f;

        float distance = direction.magnitude;

        // 🔍 ORIGEN DEL RAYCAST
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        // 👁️ RAYCASTS (frente + lados)
        Vector3 forward = transform.forward;
        Vector3 left = Quaternion.Euler(0, -30, 0) * forward;
        Vector3 right = Quaternion.Euler(0, 30, 0) * forward;

        bool wallFront = Physics.Raycast(origin, forward, wallCheckDistance, wallLayer);
        bool wallLeft = Physics.Raycast(origin, left, wallCheckDistance, wallLayer);
        bool wallRight = Physics.Raycast(origin, right, wallCheckDistance, wallLayer);

        // 🚧 SI HAY PARED → CAMBIAR DESTINO
        if (wallFront || wallLeft || wallRight)
        {
            Debug.Log("Detectando pared");
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

            // pequeño giro para despegarse
            transform.Rotate(0, Random.Range(120, 180), 0);

            target = patrolPoints[currentPoint];
            direction = (target.position - transform.position);
            direction.y = 0f;
        }

        // 🎯 SI LLEGÓ AL PUNTO → SIGUIENTE
        if (distance < pointReachDistance)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            return;
        }

        // NORMALIZAR DIRECCIÓN
        direction.Normalize();

        // 🚶 MOVIMIENTO CON FÍSICAS
        rb.MovePosition(rb.position + direction * speedNormal * Time.deltaTime);

        // 🔄 ROTACIÓN SUAVE
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.deltaTime));
        }

        // 🎬 ANIMACIÓN
        _animation.SetBool("correr", false);
    }

    //private Vector3 Wander()
    //{
    //    _animation.SetBool("correr", false);
    //    wanderTimer -= Time.deltaTime;

    //    if (wanderTimer <= 0f)
    //    {
    //        float randomAngle = Random.Range(-wanderTurnSpeed, wanderTurnSpeed);

    //        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);

    //        wanderDirection = rotation * wanderDirection;
    //        wanderDirection.y = 0f;
    //        wanderDirection.Normalize();

    //        wanderTimer = wanderChangeInterval;
    //    }

    //    return wanderDirection;
    //}
}
