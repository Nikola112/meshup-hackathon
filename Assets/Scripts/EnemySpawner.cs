using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Target[] _targets;
    [SerializeField]
    private Transform[] _spawnPoints;

    public float respawnTime = 2f;

    private ObjectPoolingSystem swarm;
    private float _timer = 0f;

    private void Awake()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            _targets[i].target = _targets[i].targetTransform.GetComponent<IDamageable>();
        }
    }

    private void Start()
    {
        swarm = GetComponent<ObjectPoolingSystem>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= respawnTime)
        {
            var enemy = swarm.Get(false);
            var enemyScript = enemy.GetComponent<BaseEnemy>();

            var target = _targets[Random.Range(0, _targets.Length)];

            enemyScript.OriginalTargetTransform = target.targetTransform;
            enemyScript.OriginalTarget = target.target;

            enemy.SetActive(true);

            _timer = 0f;
        }
    }
}
