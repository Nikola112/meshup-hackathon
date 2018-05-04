using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    protected float _timer = 0.0f;
    [SerializeField]
    protected int _health = 100;
    [SerializeField]
    protected int _damage = 10;
    [SerializeField]
    protected float _speed = 1f;
    [SerializeField]
    protected float _distanceToAttack = 0.5f;

    [SerializeField]
    protected float _timerReset = 1.0f;

    [HideInInspector]
    public GameObject TargetedPlayer;

    public int Health { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }

    public IDamageable Target { get; set; }
    public IDamageable OriginalTarget;
    public Vector3 OriginalTargetPosition;

    private void Awake()
    {
        Initalize();
    }

    protected virtual void Initalize()
    {
        Health = _health;
        Damage = _damage;
        Speed = _speed;
    }

    private void Update()
    {
        if(_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
        }

        if (Target != null)
        {
            if(Vector3.Distance(transform.position, TargetedPlayer.transform.position) < _distanceToAttack)
            {
                if(_timer <= 0.0f)
                {
                    _timer = _timerReset;
                    Attack(Target);
                }
            }
        }
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
            Death();
        }
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    public void SetOriginalTarget(IDamageable target)
    {
        OriginalTarget = target;
        OriginalTargetPosition = ((MonoBehaviour) target).transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            IPlayer player = other.gameObject.GetComponent<IPlayer>();

            if (player != null)
            {
                Target = player;
            }
        }
    }
}
