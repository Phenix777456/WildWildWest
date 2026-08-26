using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHendler : MonoBehaviour
{
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private Mover _mover;
    [SerializeField] private AnimatorController _animatorController;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] AimTargetProvider _aimTargetProvider;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private BulletSpawner _gunLeft;
    [SerializeField] private BulletSpawner _gunRight;
    [SerializeField] private MtAimFollower _mtAimFollowerLeft;
    [SerializeField] private MtAimFollower _mtAimFollowerRight;
    [SerializeField] private StartBehavior _behavior;
    [SerializeField] private UltimateLifetime _ult;
    [SerializeField] private SwordController _swordController;
    [SerializeField] private WeightController _weightController;

    private SwordThrowBehavior _swordThrowBehavior;

    private SwordComboStateBehavior _swordComboStateBehavior;

    public bool IsWithGun { get; private set; } = false;

    private Vector2 _rawInput;
    private Vector3 _direction;

    private bool _state = false;
    private bool _multiplyByTwo = true;

    private bool _isStriking;

    public event Action SwordLaunched;

    public event Action TeleportRequested;

    private void Awake()
    {
        _swordThrowBehavior = _animatorController.ReturnThrowBehavior();
        _swordComboStateBehavior = _animatorController.ReturnComboBehavior();
    }


    private void OnEnable()
    {
        _controller.MoveButtonPresed += OnMoveButtonPressed;
        _controller.RunButtonPresed += OnRunButtonPressed;
        _controller.DashButtonPresed += OnDashButtonIsPressed;
        _animatorController.DeshEnded += OnDashEnded;
        _controller.ShootLeftButtonPressed += OnShootButtonPressedLeft;
        _controller.ShootRightButtonPressed += OnShootButtonPressedRight;
        _controller.ChangeStateButtonPressed += OnChangeStageButtonPressed;
        _controller.UltButtonPressed += OnUltButtonPressed;

        StartCoroutine(MovementLoop());
    }

    private void OnDisable()
    {
        _controller.MoveButtonPresed -= OnMoveButtonPressed;
        _controller.RunButtonPresed -= OnRunButtonPressed;
        _controller.DashButtonPresed -= OnDashButtonIsPressed;
        _animatorController.DeshEnded -= OnDashEnded;
        _controller.ShootLeftButtonPressed -= OnShootButtonPressedLeft;
        _controller.ShootRightButtonPressed -= OnShootButtonPressedRight;
        _controller.ChangeStateButtonPressed -= OnChangeStageButtonPressed;
        _controller.UltButtonPressed -= OnUltButtonPressed;
        StopAllCoroutines();
    }


    public bool NextRun()
    {
        _state = !_state;
        return _state;
    }

    public float Next(float baseSpeed)
    {
        float multiplier;

        if (_multiplyByTwo)
        {
            multiplier = baseSpeed *= 2f;
        }
        else
        {
            multiplier = baseSpeed /= 2f;
        }

        _multiplyByTwo = !_multiplyByTwo;
        return  multiplier;
    }

    private void OnMoveButtonPressed(Vector2 direction)
    {
        _rawInput = direction;
    }

    private void OnUltButtonPressed()
    {
        if (_ult.StartUlt() == true)
        {
            OnGravityWork();
            _behavior.SetIsGun();
        }
    }

    private void OnRunButtonPressed()
    {
        _animatorController.SetRuning(NextRun());
        _mover.SetMoveSpeed(Next(_mover.StartMoveSpeed));   
    }

    public void OnDashButtonIsPressed()
    {
        _mover.SetIsLocked(false);
        _animatorController.SetDashTrigger();
        _mover.SetDashVelocity();

        StartCoroutine(_animatorController.DashUntillEnd());
    }

    private void OnChangeStageButtonPressed()
    {
        _behavior.CheckIsGun();
    }

    private void OnShootButtonPressedLeft()
    {
        if (IsWithGun)
        {
            _gunLeft.Fire();
            _mtAimFollowerLeft.Kick();
        }
        else if (_swordController.IsLaunched == true)
        {
            TeleportRequested?.Invoke();
        }
        else if (_swordController.IsLaunched == false)
        {
            if (_isStriking == false)
                SwordStrike();
            _isStriking = true;
            _weightController.SetRigBehavior(0);
            _animatorController.SetSwordPoseTrigger();
        }
    }

    private void SwordStrike()
    {
        _swordComboStateBehavior.StrikeFinished += OnStrikeFinished;
    }

    private void OnShootButtonPressedRight()
    {
        if (IsWithGun)
        {
            _gunRight.Fire();
            _mtAimFollowerRight.Kick();
        }
        else if (_swordController.IsLaunched == false && _isStriking == false)
        {
            _swordThrowBehavior.ThrowFinished += OnThrowFinished;
            _animatorController.SetLayerParameters(1,3);
            _animatorController.SetSwordThrowTrigger();
        }
        else
        {
            SwordLaunched?.Invoke();
            //анимация ождания возвращения
        }
    }

    private void OnStrikeFinished()
    {
        _weightController.SetRigBehavior(1);

        Debug.Log("++");

        _isStriking = false;
        _swordComboStateBehavior.StrikeFinished -= OnStrikeFinished;
    }

    private void OnThrowFinished()
    {
        _animatorController.SetLayerParameters(0, 3);
        SwordLaunched?.Invoke();
        _swordThrowBehavior.ThrowFinished -= OnThrowFinished;
    }

    private void OnDashEnded()
    {
        _mover.StopDash();
        _mover.SetIsLocked(false);
    }

    public void OnGravityWork() => _rb.isKinematic = true;
    public void OnGraviryZero() => _rb.isKinematic = false;

    private void SetToBase()
    {
        _animatorController.SetRuning(false);
        _mover.SetMoveSpeed(_mover.StartMoveSpeed);
    }

    private Vector3 CalculateCameraRelativeDirection(Vector2 rawInput)
    {
        if (rawInput.sqrMagnitude < 0.0001f)
        {
            return Vector3.zero;
        }

        Vector3 cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;

        Vector3 desiredDirection = cameraForward * rawInput.y + cameraRight * rawInput.x;

        return desiredDirection.normalized;
    }

    private IEnumerator MovementLoop()
    {
        while (enabled)
        {
            _direction = CalculateCameraRelativeDirection(_rawInput);

            bool hasInput = _direction.sqrMagnitude > 0.01f;

            Quaternion targetRotation = SetTaregetRotation(hasInput);

            bool _isRotating = Quaternion.Angle(transform.rotation, targetRotation) > 0.5f;

            if (hasInput)
            {
    
                _mover.RotateTowards(_direction);
                _mover.Move(_direction);
            }
            else
            {
                _animatorController.SetMoving(true);
                _mover.RotateBodyToAimIfOutOfDeadZone(_aimTargetProvider.CurrentYaw);
            }

            _animatorController.SetMoving(hasInput);

            if (hasInput == false)
            {
                SetToBase();
            }

            yield return null;
        }
    }

    private Quaternion SetTaregetRotation(bool hasInput)
    {
        if (hasInput)
        {
            return Quaternion.LookRotation(_direction);
        }
        else
        {
            return transform.rotation;
        }
    }

    public void SetIsWidthGun(bool isWithGun)
    {
        IsWithGun = isWithGun;
    }
}
