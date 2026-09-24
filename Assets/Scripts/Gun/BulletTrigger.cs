using System;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class BulletTrigger : MonoBehaviour
{
    [SerializeField] private ReturnBulletChannel _returnBulletChannel;
    [SerializeField] private LayerMask _layerMask;

    public event Action<BulletTrigger> EnemyHited;

    private void OnCollisionEnter(Collision collision)
    {
        _returnBulletChannel.Release(gameObject.GetComponent<Bullet>());

        bool isTargetLayer = (_layerMask.value & (1 << collision.gameObject.layer)) != 0;

        if (isTargetLayer)
            EnemyHited?.Invoke(this);
    }
}
