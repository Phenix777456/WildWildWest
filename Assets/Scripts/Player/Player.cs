using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private StartBehavior _behavior;
    [SerializeField] private PlayerInputHendler _playerInputHandler;
    [SerializeField] private SwordController _swordController;
    [SerializeField] private Mover _mover;
    [SerializeField] private AnimatorController _animatorController;

    private const float TeleportThresholdSqr = 5f;

    private void OnEnable()
    {
        _playerInputHandler.TeleportRequested += OnTeleportRequested;
    }

    private void Start()
    {
        _behavior.ChackBehavior();
    }

    private void OnDisable()
    {
        _playerInputHandler.TeleportRequested += OnTeleportRequested;
    }

    private void OnTeleportRequested()
    {
        Vector3 swordPosition = _swordController.ReturnSwordPosition();
        Vector3 toSword = gameObject.transform.position - swordPosition;

        if (toSword.sqrMagnitude <= TeleportThresholdSqr)
            return;

        swordPosition.y = 0f;

        Vector3 swordForward = _swordController.ReturnSwordForward();
        swordForward.y = 0f;

        _mover.Teleport(swordPosition);
        _swordController.HandReturner();

        if (swordForward.sqrMagnitude > 0.0001f)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(swordForward.normalized);
        }

        _playerInputHandler.OnDashButtonIsPressed();
    }
}
