using System;
using UnityEngine;

public abstract class ReturnToPoolChannel<T> : ScriptableObject
{
    public event Action<T> ObjectReleased;
    public void Release(T obj) => ObjectReleased?.Invoke(obj);
}

[CreateAssetMenu(fileName = "ReturnBulletChannel", menuName = "Channels/ReturnBulletChannel")]
public class ReturnBulletChannel : ReturnToPoolChannel<Bullet> { }

[CreateAssetMenu(fileName = "ReturnSwordChannel", menuName = "Channels/ReturnSwordChannel")]
public class ReturnSwordChannel : ReturnToPoolChannel<Sword> { }
