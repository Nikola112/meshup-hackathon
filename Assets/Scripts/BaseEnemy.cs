using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : Objective
{
    #region Fields

    private SphereCollider col;
    private NavMeshAgent agent;
    private float _timerReset = 1.0f;
    private bool end = false;

    protected float _timer = 1.0f;
    [SerializeField]
    protected int _damage = 1;
    [SerializeField]
    protected float _distanceToEngage = 5f;
    [SerializeField]
    protected float _distanceToAttack = 0.5f;
    [SerializeField]
    protected float _fireRate = 1.0f;

    public float _speed = 1f;
    public float speed;
    public EnemyType EnemyType = EnemyType.Swarm;
    
    [HideInInspector]
    public Objective target;
    [HideInInspector]
    public Objective OriginalTarget;
    [HideInInspector]
    public EnemySpawner spawner;
    [HideInInspector]
    public bool stop = false;

    #endregion

    #region Properties

    public float DistanceToEngage
    {
        get { return _distanceToEngage; }
        set
        {
            _distanceToEngage = value;
            col.radius = _distanceToEngage;
        }
    }

    #endregion

    #region Methods

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Objective player = other.gameObject.GetComponent<Objective>();

            if (player != null)
            {
                SetTarget(other.gameObject);
            }
        }
    }

    private void Update()
    {
        if (stop) return;

        if(_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
        }

        if (target != null)
        {
            if(Vector3.Distance(transform.position, target.transform.position) < _distanceToAttack)
            {
                if(_timer <= 0.0f)
                {
                    _timer = _timerReset;
                    Attack(target);
                }
            }
        }
    }

    protected override void Initalize()
    {
        base.Initalize();

        speed = _speed;

        col.radius = DistanceToEngage;
        _timerReset = 1.0f / _fireRate;

        SetOriginalTargetAsTarget();
    }

    public void SetOriginalTarget(Objective target)
    {
        if (target != null)
        {
            OriginalTarget = target;
        }
    }

    public void SetOriginalTarget(GameObject target)
    {
        Objective newTarget = target.GetComponent<Objective>();

        if (newTarget != null)
        {
            OriginalTarget = newTarget;
        }
    }

    public void SetTarget(Objective target)
    {
        if (target != null)
        {
            this.target = target;

            target.Death += OnTargetDeath;
        }
    }

    private void OnTargetDeath(object source, EventArgs args)
    {
        spawner.TargetEliminated((Objective)source);

        var newTarget = spawner.GetNewTargetAtPosition(transform.position);

        if(newTarget == null)
        {
            stop = true;
            end = true;
        }
        else
        {
            SetTarget(newTarget);
        }
    }

    public void SetTarget(GameObject target)
    {
        Objective newTarget = target.GetComponent<Objective>();

        if (newTarget != null)
        {
            this.target = newTarget;
        }
    }

    public void SetOriginalTargetAsTarget()
    {
        SetTarget(OriginalTarget);
    }

    public virtual void Attack(Objective target)
    {
        if(target == null)
        {
            return;
        }

        target.TakeDamage(_damage);
    }

    public virtual void ChangeSpeed(float percent)
    {
        speed = _speed * (percent / 100);
    }

    public void AddForce(Vector3 direction, float force)
    {
        agent.velocity = direction * force;
    }

    #endregion
}
