using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    public event Action<Vector2> MoveButtonPresed;
    public event Action RunButtonPresed;
    public event Action DashButtonPresed;
    public event Action ShootLeftButtonPressed;
    public event Action ShootRightButtonPressed;
    public event Action ChangeStateButtonPressed;
    public event Action UltButtonPressed;

    private void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();

        MoveButtonPresed?.Invoke(moveInput);
    }

    private void OnRun()
    {
        RunButtonPresed?.Invoke();
    }

    private void OnDash()
    {
        DashButtonPresed?.Invoke();
    }

    private void OnShootLeft()
    {
        ShootLeftButtonPressed?.Invoke();
    }

    private void OnShootRight()
    {
        ShootRightButtonPressed?.Invoke();
    }

    private void OnChangeStage()
    {
        ChangeStateButtonPressed?.Invoke();
    }

    private void OnUlt()
    {
        UltButtonPressed?.Invoke();
    }
}
