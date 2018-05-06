using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField]
    private int _helath;

    [HideInInspector]
    public delegate void DeathEventHandler(object source, EventArgs args);
    [HideInInspector]
    public event DeathEventHandler Death;

    public virtual void TakeDamage(int damage)
    {
        _helath -= damage;

        if(_helath <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath();

        // TEMP
        Destroy(gameObject, 1f);
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
}
