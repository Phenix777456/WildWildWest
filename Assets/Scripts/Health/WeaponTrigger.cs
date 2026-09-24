using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private float _damage = 10f;

    public event Action<float, Collider> OnHit;

    private void OnTriggerEnter(Collider other)
    {
        bool isTarget = IsTargetLayer(other.gameObject.layer);

        if (isTarget == false)
        {
            return;
        }

        OnHit?.Invoke(_damage, other);
    }

    private bool IsTargetLayer(int layer)
    {
        return (_targetLayerMask.value & (1 << layer)) != 0;
    }
}
