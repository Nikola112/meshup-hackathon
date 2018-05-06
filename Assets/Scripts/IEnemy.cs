public interface IEnemy : IDamageable, IMoveable
{
    int Damage { get; set; }

    void Attack(IDamageable target);
}
