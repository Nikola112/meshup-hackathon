using UnityEngine;
using UnityEngine.AI;

public class SwarmEnemy : MonoBehaviour, IEnemy
{
    private float _timer = 0.0f;

    [SerializeField]
    private float _timerReset = 1.0f;

    [HideInInspector]
    public GameObject TargetedPlayer;

    public int Health { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }

    public IPlayer Target { get; set; }

    private void Update()
    {
        if(_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
        }

        if (Target != null)
        {
            if(Vector3.Distance(transform.position, TargetedPlayer.transform.position) < 0.5f)
            {
                if(_timer <= 0.0f)
                {
                    _timer = _timerReset;
                    Attack(Target);
                }
            }
        }
    }

    public void Attack(IPlayer target)
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
            // Die
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IPlayer player = other.gameObject.GetComponent<IPlayer>();

            if (player != null)
            {
                Target = player;
            }
        }
    }
}
