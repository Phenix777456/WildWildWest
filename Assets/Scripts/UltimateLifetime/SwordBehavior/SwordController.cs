using System;
using System.Collections;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private PlayerInputHendler _playerInputHendler;
    [SerializeField] private Sword _targetPrefab;
    [SerializeField] private SwordMover _swordMover;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _offsetHend;
    [SerializeField] private float _dellay;
    [SerializeField] private AimTarget _aimTarget;

    private Rigidbody _targetRigidbody;

    public bool IsLaunched { get; private set; } = false;

    public event Action<SwordController, Vector3> PlayerTeleported;

    private void Awake()
    {
        _targetRigidbody = _targetPrefab.GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        _playerInputHendler.SwordLaunched += OnSwordLaunched;
    }

    private void Start()
    {
        _targetPrefab.SetSwordInHend(gameObject.transform);
    }

    private void Update()
    {
        if (IsLaunched == false && gameObject.transform.position != _targetPrefab.transform.position)
        {
            gameObject.transform.position = _targetPrefab.transform.position;
        }
    }

    private void OnDisable()
    {
        _playerInputHendler.SwordLaunched -= OnSwordLaunched;
    }

    private void OnSwordLaunched()
    {

        if (IsLaunched == false)
        {
            Vector3 direction = (_aimTarget.transform.position - gameObject.transform.position);
            direction.y = 0f;
            direction = direction.normalized;

            _targetPrefab.transform.parent = null;
            _swordMover.FastRotate(_targetRigidbody.gameObject.transform, direction, _offset);
            _swordMover.SetVelocity(_targetRigidbody, direction);

            StartCoroutine(SwordLifetime(_dellay));

            IsLaunched = true;

            _swordMover.SwordRetirned += OnSwordReturned;
        }
        else
        {
            BeginSwordReturn();
        }

    }

    private IEnumerator SwordLifetime(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        BeginSwordReturn();
    }

    private void BeginSwordReturn()
    {
        _swordMover.SetVelocity(_targetRigidbody, new Vector3(0, 0, 0));
        _swordMover.ReturnToHand(gameObject.transform);
    }

    public Vector3 ReturnSwordPosition()
    {
        return _targetPrefab.transform.position;
    }

    private void OnSwordReturned()
    {
        IsLaunched = false;
        HandReturner();
        _swordMover.SwordRetirned -= OnSwordReturned;
    }

    public Vector3 ReturnSwordForward()
    {
        if (_targetRigidbody.linearVelocity.sqrMagnitude > 0.0001f)
        {
            return _targetRigidbody.linearVelocity.normalized;
        }

        return transform.forward;
    }

    public void HandReturner()
    {
        _swordMover.SetVelocity(_targetRigidbody, new Vector3(0, 0, 0));
        _targetPrefab.gameObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(_offsetHend);
        _targetPrefab.gameObject.transform.position = gameObject.transform.position;
        _targetPrefab.transform.SetParent(gameObject.transform);

        IsLaunched = false;

        if (_targetPrefab.gameObject.transform.position != gameObject.transform.position)
            HandReturner();
    }
}
