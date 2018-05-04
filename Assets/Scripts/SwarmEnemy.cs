using UnityEngine;

public class SwarmEnemy : MonoBehaviour, IEnemy
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }

    public IPlayer Target { get; set; }

    public void Attack(IPlayer target)
    {
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
}
