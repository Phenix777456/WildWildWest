using System.Collections;
using UnityEngine;

public class MtAimFollower : MonoBehaviour
{
    [SerializeField] private AimTargetProvider _aimTargetProvider;
    [SerializeField] private Transform _shoulderAnchor;
    [SerializeField] private Transform _handAnchor;
    [SerializeField] private Vector3 _lateralOffset = new Vector3(0.15f, -0.05f, 0f);
    [SerializeField] private float _reachDistance = 0.5f;
    [SerializeField] private float _maxHandDistance = 0.2f;
    [SerializeField] private bool _isRightHand = true;

    [Header("Recoil")]
    [SerializeField] private float _recoilReachDistance = 0.4f;
    [SerializeField] private float _recoilKickTime = 0.04f;
    [SerializeField] private float _recoilReturnTime = 0.15f;

    private float _baseReachDistance;
    [SerializeField] private float _currentReachDistance;
    private Coroutine _recoilCoroutine;

    private void Awake()
    {
        _baseReachDistance = _reachDistance;
        _currentReachDistance = _reachDistance;
    }

    private void LateUpdate()
    {
        Vector3 aimDirection = (_aimTargetProvider.CurrentAimPoint - _shoulderAnchor.position).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection, Vector3.up);

        Vector3 mirroredOffset = _lateralOffset;
        mirroredOffset.x = _isRightHand ? Mathf.Abs(_lateralOffset.x) : -Mathf.Abs(_lateralOffset.x);
        Vector3 rotatedOffset = aimRotation * mirroredOffset;

        Vector3 desiredPosition = _shoulderAnchor.position + aimDirection * _currentReachDistance + rotatedOffset;

        transform.position = ClampToHandReach(desiredPosition);
        transform.rotation = aimRotation;
    }

    private Vector3 ClampToHandReach(Vector3 desiredPosition)
    {
        if (_handAnchor == null)
        {
            return desiredPosition;
        }

        Vector3 handToDesired = desiredPosition - _handAnchor.position;
        return _handAnchor.position + Vector3.ClampMagnitude(handToDesired, _maxHandDistance);
    }

    public void Kick()
    {
        if (_recoilCoroutine != null)
        {
            StopCoroutine(_recoilCoroutine);
        }

        _recoilCoroutine = StartCoroutine(PlayRecoil());
    }

    private IEnumerator PlayRecoil()
    {
        yield return LerpReachDistance(_baseReachDistance, _recoilReachDistance, _recoilKickTime);
        yield return LerpReachDistance(_recoilReachDistance, _baseReachDistance, _recoilReturnTime);
    }

    private IEnumerator LerpReachDistance(float from, float to, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            _currentReachDistance = Mathf.Lerp(from, to, progress);
            yield return null;
        }

        _currentReachDistance = to;
    }
}