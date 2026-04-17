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
}
