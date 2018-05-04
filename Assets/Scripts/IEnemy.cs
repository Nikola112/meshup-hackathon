public interface IEnemy : IDamageable, IMoveable
{
    int Damage { get; set; }
    IDamageable Target { get; set; }

    void Attack(IDamageable target);
}
