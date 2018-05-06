using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Fields

    private SphereCollider col;
    private PooledObject pool;
    private float _timerReset = 1.0f;

    protected float _timer = 1.0f;
    [SerializeField]
    protected int _health = 100;
    [SerializeField]
    protected int _damage = 10;
    [SerializeField]
    protected float _speed = 1f;
    [SerializeField]
    protected float _distanceToEngage = 5f;
    [SerializeField]
    protected float _distanceToAttack = 0.5f;
    [SerializeField]
    protected float _fireRate = 1.0f;

    [HideInInspector]
    public Transform TargetTransform;
    [HideInInspector]
    public Objective Target;
    [HideInInspector]
    public Objective OriginalTarget;
    [HideInInspector]
    public Transform OriginalTargetTransform;
    [HideInInspector]
    public delegate void DeathEventHandler(object source, EventArgs args);
    [HideInInspector]
    public event DeathEventHandler Death;

    #endregion

    #region Properties

    public int Health { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }

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
    }

    private void Start()
    {
        Initalize();
        SetOriginalTargetAsTarget();

        pool = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        Initalize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Objective player = other.gameObject.GetComponent<Objective>();

            if (player != null)
            {
                SetTarget(other.gameObject);

                this.Death += EnemyDeath;
            }
        }
    }

    private void EnemyDeath(object source, EventArgs args)
    {
    }

    private void Update()
    {
        if(_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
        }

        if (Target != null)
        {
            if(Vector3.Distance(transform.position, TargetTransform.position) < _distanceToAttack)
            {
                if(_timer <= 0.0f)
                {
                    _timer = _timerReset;
                    Attack(Target);
                }
            }

            if(Target.Health <= 0)
            {
                SetOriginalTargetAsTarget();
            }
        }
    }

    protected virtual void Initalize()
    {
        col.radius = DistanceToEngage;
        _timerReset = 1.0f / _fireRate;

        Health = _health;
        Damage = _damage;
        Speed = _speed;

        SetOriginalTargetAsTarget();
    }

    public void SetOriginalTarget(GameObject target, Transform differentTransform = null)
    {
        Objective damageable = target.GetComponent<Objective>();

        if (damageable != null)
        {
            OriginalTarget = target.GetComponent<Objective>();

            if (differentTransform == null) TargetTransform = target.transform;
            else TargetTransform = differentTransform;
        }
    }

    public void SetTarget(GameObject target, Transform differentTransform = null)
    {
        Objective damageable = target.GetComponent<Objective>();

        if (damageable != null)
        {
            Target = target.GetComponent<Objective>();

            if (differentTransform == null) TargetTransform = target.transform;
            else TargetTransform = differentTransform;
        }
    }

    public void SetOriginalTargetAsTarget()
    {
        Target = OriginalTarget;
        TargetTransform = OriginalTargetTransform;
    }

    public virtual void Attack(Objective target)
    {
        if(target == null)
        {
            return;
        }

        target.TakeDamage(Damage);
    }

    public virtual void ChangeSpeed(float percent)
    {
        Speed = Speed * (percent / 100);
    }

    public virtual void TakeDamage(int damage, bool armorPiercing = false)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath();
        if (pool != null)
        {
            pool.Return();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    protected virtual void OnDeath()
    {
        if (Death != null)
        {
            Death(this, EventArgs.Empty);

            foreach (Delegate d in Death.GetInvocationList())
            {
                Death -= (DeathEventHandler)d;
            }
        }
    }

    #endregion
}
