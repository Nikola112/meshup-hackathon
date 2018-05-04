public interface IEnemy : IDamageable, IMoveable
{
    int Damage { get; set; }
    IPlayer Target { get; set; }

    void Attack(IPlayer target);
}
