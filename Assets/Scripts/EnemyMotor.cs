using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : MonoBehaviour
{
    private float _timer;
    [SerializeField]
    private float _timerReset = 0.5f;

    private IEnemy _thisEnemy;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _thisEnemy = GetComponent<IEnemy>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.speed *= _thisEnemy.Speed;
    }

    private void LateUpdate()
    {
        if(_timer >= _timerReset)
        {
            _timer = 0.0f;

            //_agent.SetDestination(((MonoBehaviour)_thisEnemy.Target));
        }
    }
}
