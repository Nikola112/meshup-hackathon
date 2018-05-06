using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private PooledObject pool;

    [SerializeField]
    protected int _helath;

    [HideInInspector]
    public delegate void DeathEventHandler(object source, EventArgs args);
    [HideInInspector]
    public event DeathEventHandler Death;

    private void Start()
    {
        Initalize();
        pool = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        Initalize();
    }

    protected virtual void Initalize() { }

    public virtual void TakeDamage(int damage, bool armorPiercing = false)
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
}
