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

    private void Awake()
    {
        Initalize();
    }

    private void Update()
    {
        _agent.speed *= _thisEnemy.Speed;
    }

    private void LateUpdate()
    {
        _timer += Time.deltaTime;

        if(_timer >= _timerReset)
        {
            _timer = 0.0f;

            _agent.SetDestination(_thisEnemy.TargetTransform.position);
        }
    }

    private void Initalize()
    {
        _thisEnemy = GetComponent<BaseEnemy>();
        _agent = GetComponent<NavMeshAgent>();

        _timerReset = 1f / _refreshRate;
        _timer = _timerReset * 0.9f;
    }

    private void OnEnable()
    {
        Initalize();
    }
}
