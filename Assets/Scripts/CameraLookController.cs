using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    [SerializeField] private AimTargetProvider _aimTargetProvider;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _rotation = new Vector3(0, 30, 0);
    [SerializeField] private Vector3 _targetOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private float _distance = 4f;

    private bool _isLocked = false;

    private Vector3 _rotationBase;
    private Vector3 _offsetBase;

    private void Awake()
    {
        _rotationBase = _rotation;
        _offsetBase = _targetOffset;
    }

    private void LateUpdate()
    {
        if (_target == null || _aimTargetProvider == null)
            return;

        Quaternion baseRotation = _isLocked ? _target.rotation : _aimTargetProvider.CurrentLookRotation;
        Quaternion rotation = baseRotation * Quaternion.Euler(_rotation);

        Vector3 pivotPosition = _target.position + _targetOffset;
        Vector3 desiredPosition = pivotPosition - rotation * Vector3.forward * _distance;

        transform.rotation = rotation;
        transform.position = desiredPosition;
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