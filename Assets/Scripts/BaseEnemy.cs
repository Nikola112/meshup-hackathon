using System;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
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
    public IDamageable Target;
    [HideInInspector]
    public IDamageable OriginalTarget;
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
            IPlayer player = other.gameObject.GetComponent<IPlayer>();

            if (player != null)
            {
                SetTarget(other.gameObject);
            }
        }
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
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            OriginalTarget = target.GetComponent<IDamageable>();

            if (differentTransform == null) TargetTransform = target.transform;
            else TargetTransform = differentTransform;
        }
    }

    public void SetTarget(GameObject target, Transform differentTransform = null)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Target = target.GetComponent<IDamageable>();

            if (differentTransform == null) TargetTransform = target.transform;
            else TargetTransform = differentTransform;
        }
    }

    public void SetOriginalTargetAsTarget()
    {
        Target = OriginalTarget;
        TargetTransform = OriginalTargetTransform;
    }

    public void Attack(IDamageable target)
    {
        if(target == null)
        {
            return;
        }

        target.TakeDamage(Damage);
    }

    public void ChangeSpeed(float percent)
    {
        Speed = Speed * (percent / 100);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    protected void Die()
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
