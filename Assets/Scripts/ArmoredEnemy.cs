using UnityEngine;

public class ArmoredEnemy : BaseEnemy
{
    [SerializeField]
    protected int _armor = 10;

    public override void TakeDamage(int damage, bool armorPiercing = false)
    {
        damage = armorPiercing ? damage - _armor : damage;
        base.TakeDamage(damage);
    }
}
