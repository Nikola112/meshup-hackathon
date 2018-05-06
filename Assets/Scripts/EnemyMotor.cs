using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : MonoBehaviour
{
    private float _timer;
    private float _timerReset;
    [SerializeField]
    private float _refreshRate = 2f;

    private BaseEnemy _thisEnemy;
    private NavMeshAgent _agent;
    private float _initialSpeed;

    private void Awake()
    {
        Initalize();
    }

    private void Update()
    {
        _agent.isStopped = _thisEnemy.stop;

        if (_thisEnemy.stop) return;

        _agent.speed = _initialSpeed * _thisEnemy.speed;
    }

    private void LateUpdate()
    {
        if (_thisEnemy.stop) return;

        _timer += Time.deltaTime;

        if(_timer >= _timerReset)
        {
            _timer = 0.0f;

            _agent.SetDestination(_thisEnemy.target.transform.position);
        }
    }

    private void Initalize()
    {
        _thisEnemy = GetComponent<BaseEnemy>();
        _agent = GetComponent<NavMeshAgent>();

        _timerReset = 1f / _refreshRate;
        _timer = _timerReset * 0.9f;
        _initialSpeed = _agent.speed;
    }

    private void OnEnable()
    {
        Initalize();
    }
}
