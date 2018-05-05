using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private Transform[] _targets;

    public float respawnTime = 2f;

    private ObjectPoolingSystem swarm;
    private float _timer = 0f;

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

            enemyScript.OriginalTargetTransform = target;
            enemyScript.OriginalTarget = target.GetComponent<IDamageable>();

            enemy.SetActive(true);

            _timer = 0f;
        }
    }
}
