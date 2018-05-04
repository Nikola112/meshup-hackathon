using UnityEngine;

public interface IPlayer : IDamageable, IMoveable
{
    GameObject ThisGameObject { get; set; }
}
