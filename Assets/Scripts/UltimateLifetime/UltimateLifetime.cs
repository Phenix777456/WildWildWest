using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class UltimateLifetime : MonoBehaviour
{
    [SerializeField] private AnimatorController _animatorController;
    [SerializeField] private PointsControoller _pointsControoller;
    [SerializeField] private Mover _mover;
    [SerializeField] private CameraLookController _cameraLookController;
    [SerializeField] private SwordSpawner _spawner;

    [Header("Ult Parems")]
    [SerializeField] private float _speedOfFlying;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _dellayHendSweep;
    [SerializeField] private float _dellaySwordSpawn;
    [SerializeField] private float _cameraDistance;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _minVelocity;
    [SerializeField] private Vector3 _rotation;

    private SwordAppearStateBehaviour _appearStateBehaviour;

    private bool _isReaduToUlt = false;

    private void Awake()
    {
        _appearStateBehaviour = _animatorController.ReturnAppearBehavior();
    }

    private void OnEnable()
    {
        _pointsControoller.SpBarFilled += OnSpBarFilled;
        _appearStateBehaviour.AppearFinished += OnAppearFinished;
    }

    private void OnDisable()
    {
        _appearStateBehaviour.AppearFinished -= OnAppearFinished;
        _pointsControoller.SpBarFilled -= OnSpBarFilled;
    }

    private void OnSpBarFilled()
    {
        _isReaduToUlt = true;
    }

    public bool StartUlt()
    {
        if (_isReaduToUlt == false)
            return false;

        _mover.TargetReached += OnTargetReached;

        _cameraLookController.SetIsLocked(true);
        _cameraLookController.SetDistance(_cameraDistance);
        _mover.SetIsLocked(true);
        _animatorController.SetFloating(true);

        _mover.MoveByDistance(_targetPosition, _speedOfFlying);

        return true;
    }

    private void OnTargetReached()
    {
        _mover.TargetReached -= OnTargetReached;

        StartCoroutine(DelayedAction(_dellayHendSweep));  
    }

    private IEnumerator DelayedAction(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        _animatorController.PlayRightHandAnimation();
    }

    private IEnumerator DelayedAction2(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        _spawner.StartSpawning();
        _spawner.SwordsSpawned += OnSwordsSpawned;
    }

    private void OnSwordsSpawned()
    {
        _mover.FastRotate(_rotation);

        _animatorController.SetSendSwordFlyinTrigger();

        foreach (Sword sword in _spawner.GetSwordsList())
        {
            SwordMover swordMover = sword.GetComponent<SwordMover>();
            Rigidbody swordRb = sword.GetComponent<Rigidbody>();

            swordRb.isKinematic = false;

            //swordMover.SetRotation(sword, _rotation);
            swordMover.SetVelocity(swordRb, sword.transform.up, Random.Range(_minVelocity, _maxVelocity));
        }

        _spawner.SwordsSpawned -= OnSwordsSpawned;
    }
    private void OnAppearFinished()
    {
        StartCoroutine(DelayedAction2(_dellaySwordSpawn));
    }


}
