using System;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;

    public event Action GroundTouched;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsGroundLayer(collision.gameObject.layer) == false)
            return;

        GroundTouched?.Invoke();
    }

    private bool IsGroundLayer(int layer)
    {
        return (_groundLayerMask.value & (1 << layer)) != 0;
    }

}