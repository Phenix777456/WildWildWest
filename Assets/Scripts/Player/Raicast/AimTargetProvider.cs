using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AimTargetProvider : MonoBehaviour
{
    [SerializeField] private Transform _aimOrigin;
    [SerializeField] private float _aimDistance = 50f;

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseSensitivity = 0.15f;

    [Header("Gamepad Stick Settings")]
    [SerializeField] private float _stickSensitivity = 120f;
    [SerializeField] private float _stickDeadzone = 0.15f;

    [Header("Angle Limits")]
    [SerializeField] private float _minPitch = -60f;
    [SerializeField] private float _maxPitch = 60f;

    [Header("Smoothing")]
    [SerializeField] private float _aimSmoothTime = 0.06f;

    [Header("Startup Alignment")]
    [SerializeField] private Transform _forwardReference;

    [Header("Cursor Lock")]
    [SerializeField] private bool _lockCursorOnStart = true;
    [SerializeField] private int _framesToDiscardAfterLock = 1;

    [Header("Aim Marker")]
    [SerializeField] private Transform _aimMarker;

    public Vector3 CurrentAimPoint { get; private set; }
    public Quaternion CurrentLookRotation { get; private set; }
    public bool IsAimLocked { get; private set; }
    public float CurrentYaw => _yaw;

    private float _yaw;
    private float _pitch;
    private Vector3 _rawAimPoint;
    private Vector3 _smoothVelocity;
    private int _discardFramesRemaining;

    public event Action<Vector3> FireDirectionRequested;

    private void Reset()
    {
        _aimOrigin = transform;
    }

    private void Start()
    {
        AlignToForwardReference();

        CurrentLookRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        _rawAimPoint = _aimOrigin.position + CurrentLookRotation * Vector3.forward * _aimDistance;
        CurrentAimPoint = _rawAimPoint;

        if (_lockCursorOnStart)
        {
            LockAndResetCursor();
        }
    }

    private void LockAndResetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Первые N кадров после захвата отбрасываем инпут мыши,
        // чтобы накопленный/скачковый delta не сдвинул _yaw/_pitch.
        _discardFramesRemaining = _framesToDiscardAfterLock;

        if (Mouse.current != null)
        {
            InputSystem.ResetDevice(Mouse.current);
        }
    }

    public void ReleaseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public Vector3 GetFireDirectionFrom(Vector3 muzzlePosition)
    {
        return (CurrentAimPoint - muzzlePosition).normalized;
    }

    public void RequestFireDirection(Vector3 muzzlePosition)
    {
        Vector3 direction = GetFireDirectionFrom(muzzlePosition);
        FireDirectionRequested?.Invoke(direction);
    }

    private void AlignToForwardReference()
    {
        Transform reference = _forwardReference != null ? _forwardReference : _aimOrigin;

        Vector3 horizontalForward = Vector3.ProjectOnPlane(reference.forward, Vector3.up);

        if (horizontalForward.sqrMagnitude < 0.0001f)
        {
            horizontalForward = Vector3.forward;
        }

        Quaternion levelRotation = Quaternion.LookRotation(horizontalForward.normalized, Vector3.up);

        _yaw = levelRotation.eulerAngles.y;
        _pitch = 0f;
    }

    private void Update()
    {
        if (IsAimLocked == false)
        {
            Vector2 lookInput = ReadLookInput();
            ApplyLookInput(lookInput);
        }

        UpdateAimPointFromLookRotation();

        CurrentAimPoint = Vector3.SmoothDamp(
            CurrentAimPoint, _rawAimPoint, ref _smoothVelocity, _aimSmoothTime);
    }

    private void LateUpdate()
    {
        UpdateAimMarker();
    }

    private void UpdateAimMarker()
    {
        if (_aimMarker == null)
        {
            return;
        }

        _aimMarker.position = CurrentAimPoint;
    }

    private Vector2 ReadLookInput()
    {
        if (_discardFramesRemaining > 0)
        {
            _discardFramesRemaining--;
            ConsumeRawMouseDelta();
            return Vector2.zero;
        }

        Vector2 stickInput = ReadStickInput();

        if (stickInput.sqrMagnitude > _stickDeadzone * _stickDeadzone)
        {
            return stickInput * _stickSensitivity * Time.deltaTime;
        }

        return ReadMouseInput() * _mouseSensitivity;
    }

    private void ConsumeRawMouseDelta()
    {
        if (Mouse.current != null)
        {
            Mouse.current.delta.ReadValue();
        }
    }

    private Vector2 ReadStickInput()
    {
        if (Gamepad.current == null)
        {
            return Vector2.zero;
        }

        return Gamepad.current.rightStick.ReadValue();
    }

    private Vector2 ReadMouseInput()
    {
        if (Mouse.current == null || IsPointerOverUI())
        {
            return Vector2.zero;
        }

        return Mouse.current.delta.ReadValue();
    }

    private void ApplyLookInput(Vector2 lookInput)
    {
        _yaw += lookInput.x;
        _pitch -= lookInput.y;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        CurrentLookRotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void UpdateAimPointFromLookRotation()
    {
        Vector3 direction = CurrentLookRotation * Vector3.forward;
        _rawAimPoint = _aimOrigin.position + direction * _aimDistance;
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
    }
}