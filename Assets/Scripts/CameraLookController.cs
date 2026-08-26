using Unity.VisualScripting;
using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    [SerializeField] private AimTargetProvider _aimTargetProvider;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _rotation = new Vector3(0, 30, 0);
    [SerializeField] private Vector3 _targetOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private float _distance = 4f;

    private bool _isLocked = false;

    private bool _isCinematic = false;

    private void LateUpdate()
    {
        if (_target == null || _aimTargetProvider == null)
            return;

        if (_isCinematic)
        {
            return;
        }

        SetCameraParans();
    }

    private void SetCameraParans()
    {
        Quaternion baseRotation = _isLocked ? _target.rotation : _aimTargetProvider.CurrentLookRotation;
        Quaternion rotation = baseRotation * Quaternion.Euler(_rotation);

        Vector3 pivotPosition = _target.position + _targetOffset;
        Vector3 desiredPosition = pivotPosition - rotation * Vector3.forward * _distance;

        transform.rotation = rotation;
        transform.position = desiredPosition;
    }

    public void SetOffsetTooUlt(float newDistance, Vector3 newRotatiion, Vector3 newOffset, Player player)
    {
        _distance = newDistance;
        _rotation = newRotatiion;
        _targetOffset = newOffset;

        SetCameraParans();

        if (player != null)
        {
            gameObject.transform.parent = player.transform;

        }
        else
        {
            gameObject.transform.parent = null;
            _isCinematic = true;
        }
    }



    public void SetIsLocked(bool isLocked)
    {
        _isLocked = isLocked;
    }

    public void SetRotation(Vector3 rotation)
    {
        _rotation = rotation;
    }

    public void SetDistance(float distance)
    {
        _distance = distance;
    }
}