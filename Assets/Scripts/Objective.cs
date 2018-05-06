using UnityEngine;

public class Objective : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage");
    }
}
