using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dellayDash = 0.5f;
    [SerializeField] private string _rightHandStateName = "Start";
    [SerializeField] private int _rightHandLayerIndex = 3; 

    private static readonly int IsMovingHash  =  Animator.StringToHash("IsMoving");
    private static readonly int IsRunningHash  = Animator.StringToHash("IsRunning");
    private static readonly int IsFloatingHash = Animator.StringToHash("IsFloating");
    private static readonly int DashTrigger = Animator.StringToHash("Dash");
    private static readonly int ShootTrigger = Animator.StringToHash("Shoot");
    private static readonly int ShakeHendTrigger = Animator.StringToHash("ShakeHend");
    private static readonly int SwordPoseTrigger = Animator.StringToHash("SwordPose");
    private static readonly int SwordThrowTrigger = Animator.StringToHash("ThrowTrigger");
    private static readonly int SendSwordFlyinfTrigger = Animator.StringToHash("SendSwordsTrigger");

    private static readonly int UpperLayer = 1;
    private static readonly int RightHendLayer = 2;


    private float _scaleMin = 0.3f;
    private float _scaleMax = 1f;

    public event Action DeshEnded;

    public void SetMoving(bool isMoving)
    {
        _animator.SetBool(IsMovingHash, isMoving);
    }

    public void SetRuning(bool isRunning)
    {
        _animator.SetBool(IsRunningHash, isRunning);
    }

    public void SetDashTrigger()
    {
        _animator.SetTrigger(DashTrigger);
    }

    public void SetSendSwordFlyinTrigger()
    {
        _animator.SetTrigger(SendSwordFlyinfTrigger);
    }

    public void SetShootTrigger()
    {
        _animator.SetTrigger(ShootTrigger);
    }

    public void SetSwordPoseTrigger()
    {
        _animator.SetTrigger(SwordPoseTrigger);
    }

    public void SetFloating(bool isFloating)
    {
        _animator.SetBool(IsFloatingHash, isFloating);
    }

    public void SetSwordThrowTrigger()
    {
        _animator.SetTrigger(SwordThrowTrigger);
    }

    public void SetLayerParameters(float target, int layerNumber)
    {
        _animator.SetLayerWeight(layerNumber, target);
    }

    public void PlayRightHandAnimation()
    {
        _animator.SetLayerWeight(_rightHandLayerIndex, 1);
        _animator.SetTrigger(ShakeHendTrigger);
    }

    public IEnumerator DashUntillEnd()
    {
        yield return new WaitForSeconds(_dellayDash);
         
        DeshEnded?.Invoke();
    }

    public void SetRootMotion(bool isEnabled)
    {
        _animator.applyRootMotion = isEnabled;
    }

    public SwordThrowBehavior ReturnThrowBehavior()
    {
        return _animator.GetBehaviour<SwordThrowBehavior>();
    }

    public SwordComboStateBehavior ReturnComboBehavior()
    {
        return _animator.GetBehaviour<SwordComboStateBehavior>();
    }

    public SwordAppearStateBehaviour ReturnAppearBehavior()
    {
        return _animator.GetBehaviour<SwordAppearStateBehaviour>();
    }
}