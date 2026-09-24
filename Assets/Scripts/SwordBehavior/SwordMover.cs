using System;
using System.Collections;
using UnityEngine;

public class SwordMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;

    private Coroutine _moveRoutine;

    public event Action SwordRetirned;

    public void FastRotate(Transform targetPrefab, Vector3 direction, Vector3 offset)
    {
        Quaternion baseRotation = Quaternion.LookRotation(direction);
        Quaternion offsetRotation = Quaternion.Euler(offset);
        targetPrefab.rotation = baseRotation * offsetRotation;
    }

    public void SetVelocity(Rigidbody target, Vector3 direction)
    {
        target.linearVelocity = direction * _speed;
    }

    public void SetVelocity(Rigidbody target, Vector3 direction, float speed)
    {
        target.linearVelocity = direction.normalized * speed;
    }

    public void SetRotation(Sword target, Vector3 direction)
    {
        target.transform.Rotate(direction);
    }

    public void ReturnToHand(Transform hendTransform)
    {

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveRoutine(hendTransform.position, hendTransform));
    }

    private IEnumerator MoveRoutine(Vector3 targetPosition, Transform hendTransform)
    {
        const float thresholdSqr = 0.1f;

        while ((gameObject.transform.position - targetPosition).sqrMagnitude > thresholdSqr)
        {
            if (targetPosition != hendTransform.position)
                targetPosition = hendTransform.position;

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, _speed * Time.deltaTime);
            Quaternion lookRotation = Quaternion.LookRotation((targetPosition - gameObject.transform.position).normalized);
            Quaternion visualOffset = Quaternion.Euler(_offset);

            gameObject.transform.rotation = lookRotation * visualOffset;
            yield return null;
        }

        SwordRetirned?.Invoke();

        gameObject.transform.position = targetPosition;
        _moveRoutine = null;
    }
}
