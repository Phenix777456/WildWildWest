using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;

    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _bodyRotationSpeed = 480f;
    [SerializeField] private float _moveDuration = 0.5f;

    public bool IsMoving { get; private set; } = false;

    [Header("Aim Dead Zone")]
    [SerializeField] private float _aimDeadZoneAngle = 70f;
    [SerializeField] private float _deadZoneRotationSpeed = 300f;

    public float StartMoveSpeed => _moveSpeed;

    private bool _isLocked = false;


    private Coroutine _moveRoutine;

    private Rigidbody _playerRb;

    public event Action TargetReached;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        if (_isLocked) 
            return;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Vector3 motion = direction.normalized * _moveSpeed * Time.deltaTime;

        gameObject.transform.position += motion;
    }

    public void FastRotate(Vector3 direction)
    {
        gameObject.transform.Rotate(direction);
    }

    public void RotateTowards(Vector3 direction)
    {
        if (_isLocked)
            return;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, _bodyRotationSpeed * Time.deltaTime);
    }

    public void RotateBodyToAimIfOutOfDeadZone(float aimYaw)
    {
        if (_isLocked)
            return;

        float currentYaw = transform.eulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(currentYaw, aimYaw);

        if (Mathf.Abs(angleDifference) <= _aimDeadZoneAngle)
        {
            return;
        }

        float targetYaw = Mathf.MoveTowardsAngle(
            currentYaw, aimYaw, _deadZoneRotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, targetYaw, 0f);
    }

    public void MoveByDistance(Vector3 offset, float speed)
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        Vector3 targetPosition = transform.position + offset;
        _moveRoutine = StartCoroutine(MoveRoutine(targetPosition, speed));
    }

    public void SetDashVelocity(float timing)
    {
        _playerRb.linearVelocity = transform.forward * _dashSpeed;

        if (timing != -1)
            StartCoroutine(DashDellay(timing));
    }

    public void StopDash()
    {
        _playerRb.linearVelocity = Vector3.zero;
    }

    private IEnumerator MoveRoutine(Vector3 targetPosition, float speed)
    {
        const float thresholdSqr = 0.1f;

        while ((transform.position - targetPosition).sqrMagnitude > thresholdSqr)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        TargetReached.Invoke();

        transform.position = targetPosition;
        _moveRoutine = null;
    }

    private IEnumerator DashDellay(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        _playerRb.linearVelocity = Vector3.zero;
    }

    public bool IsAimOutOfDeadZone(float aimYaw)
    {
        float currentYaw = transform.eulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(currentYaw, aimYaw);
        return Mathf.Abs(angleDifference) > _aimDeadZoneAngle;
    }

    public void SetIsLocked(bool isLocked)
    {
        _isLocked = isLocked;
    }

    public void SetIsMoving(bool isMoving)
    {
        IsMoving = isMoving;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void Teleport(Vector3 targetPosition)
    {
        gameObject.transform.position = targetPosition;
    }
}
