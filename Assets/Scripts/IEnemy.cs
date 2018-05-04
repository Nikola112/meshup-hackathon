public interface IEnemy : IDamageable, IMoveable
{
    int Damage { get; set; }

    void Attack(IPlayer target);
}
