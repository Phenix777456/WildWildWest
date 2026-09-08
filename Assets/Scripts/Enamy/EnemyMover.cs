using System;
using System.Collections;
using UnityEngine;

public class EnamyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _stoppingDistance;

    public event Action TargetReached;

    public bool IsMoving { get; private set; }
    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Max(0f, value);
    }

    private Coroutine _moveRoutine;

    public void MoveTo(Transform targetTransform)
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        _moveRoutine = StartCoroutine(MoveRoutine(targetTransform));
    }

    public void Stop()
    {
        if (_moveRoutine == null)
        {
            return;
        }

        StopCoroutine(_moveRoutine);
        _moveRoutine = null;
        IsMoving = false;
    }

    private IEnumerator MoveRoutine(Transform targetTransform)
    {
        IsMoving = true;

        Vector3 currentPosition = targetTransform.position;

        while (HasReachedTarget(currentPosition) == false)
        {
            currentPosition = targetTransform.position;

            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, step);
            transform.LookAt(currentPosition);
            yield return null;
        }

        IsMoving = false;
        _moveRoutine = null;
        TargetReached?.Invoke();
    }

    public bool HasReachedTarget(Vector3 targetPosition)
    {
        return (transform.position - targetPosition).sqrMagnitude <= _stoppingDistance;
    }
}