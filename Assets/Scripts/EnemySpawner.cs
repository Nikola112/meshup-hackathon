using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<Objective> _targets;
    [SerializeField]
    private Transform[] _spawnPoints;

    private ObjectPoolingSystem swarm;
    private float _timer = 0f;

    public float respawnTime = 2f;

    private int num = 0;

    private void Start()
    {
        swarm = GetComponent<ObjectPoolingSystem>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= respawnTime && _targets.Count > 0 && num < 10)
        {
            var enemy = swarm.Get(_spawnPoints[Random.Range(0, _spawnPoints.Length)].position, Quaternion.identity, false);
            var enemyScript = enemy.GetComponent<BaseEnemy>();

            var target = _targets[Random.Range(0, _targets.Count)];

            enemyScript.OriginalTarget = target;
            enemyScript.spawner = this;

            enemy.SetActive(true);

            num++;

            _timer = 0f;
        }
    }

    public Objective GetNewTargetAtPosition(Vector3 pos)
    {
        if (_targets.Count == 0) return null;

        Objective theChosenOne = _targets[0];
        float dist = Vector3.Distance(pos, theChosenOne.transform.position);

        for (int i = 1; i < _targets.Count; i++)
        {
            float curDist = Vector3.Distance(pos, _targets[i].transform.position);
            if (curDist < dist)
            {
                theChosenOne = _targets[0];
                dist = curDist;
            }
        }

        return theChosenOne;
    }

    public void TargetEliminated(Objective target)
    {
        _targets.Remove(target);

        if(_targets.Count == 0)
        {
            // TODO: Game Over
        }
    }
}
