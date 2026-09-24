using System.Collections;
using UnityEngine;

public class UltimateLifetime : MonoBehaviour
{
    [SerializeField] private AnimatorController _animatorController;
    [SerializeField] private PointsControoller _pointsControoller;
    [SerializeField] private Mover _mover;
    [SerializeField] private CameraLookController _cameraLookController;
    [SerializeField] private SwordSpawner _spawner;
    [SerializeField] private EnemyProximityDetector _enemyProximityDetector;

    [Header("Flight")]
    [SerializeField] private float _flightSpeed;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _flightRotation;

    [Header("Timings")]
    [SerializeField] private float _handSweepDelay;
    [SerializeField] private float _swordSpawnDelay;

    [Header("Camera")]
    [SerializeField] private float _cameraDistance;
    [SerializeField] private Vector3 _cameraRotation;
    [SerializeField] private Vector3 _cameraTargetOffset;
    [SerializeField] private float _cameraTargetDistance;

    [Header("Sword Throw")]
    [SerializeField] private float _minSwordVelocity;
    [SerializeField] private float _maxSwordVelocity;

    private SwordAppearStateBehaviour _appearStateBehaviour;

    private bool _isReadyToUlt;

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
        _isReadyToUlt = true;
    }

    public bool StartUlt()
    {
        if (_isReadyToUlt == false)
            return false;

        _mover.TargetReached += OnTargetReached;

        _enemyProximityDetector.DetectEnemies();

        _cameraLookController.SetIsLocked(true);
        _cameraLookController.SetDistance(_cameraDistance);
        _mover.SetIsLocked(true);
        _animatorController.SetFloating(true);

        _mover.MoveByDistance(_targetPosition, _flightSpeed);

        return true;
    }

    private void OnTargetReached()
    {
        _mover.TargetReached -= OnTargetReached;

        StartCoroutine(PlayHandSweepAfterDelay(_handSweepDelay));

        _cameraLookController.SetOffsetTooUlt(_cameraTargetDistance, _cameraRotation, _cameraTargetOffset, null);
    }

    private IEnumerator PlayHandSweepAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animatorController.PlayRightHandAnimation();
    }

    private IEnumerator SpawnSwordsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _spawner.SwordsSpawned += OnSwordsSpawned;
        _spawner.StartSpawning();
    }

    private void OnSwordsSpawned()
    {
        _spawner.SwordsSpawned -= OnSwordsSpawned;

        _mover.FastRotate(_flightRotation);
        _animatorController.SetSendSwordFlyinTrigger();

        foreach (Sword sword in _spawner.GetSwordsList())
        {
            SwordMover swordMover = sword.GetComponent<SwordMover>();
            Rigidbody swordRigidbody = sword.GetComponent<Rigidbody>();

            swordRigidbody.isKinematic = false;

            float velocity = Random.Range(_minSwordVelocity, _maxSwordVelocity);
            swordMover.SetVelocity(swordRigidbody, sword.transform.rotation * Vector3.up, velocity);
            Debug.Log($"up={sword.transform.up}, right={sword.transform.right}, forward={sword.transform.forward}");
        }
    }

    private void OnAppearFinished()
    {
        StartCoroutine(SpawnSwordsAfterDelay(_swordSpawnDelay));
    }
}
