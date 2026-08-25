using System;
using UnityEngine;

public class StartBehavior : MonoBehaviour
{
    private enum BehaviorState { Free, Gun, Sword }
    private BehaviorState _state;

    [SerializeField] private PlayerInputHendler _playerInputHendler;
    [SerializeField] private WeightController _weightController;
    [SerializeField] private Gun _gunLeft;
    [SerializeField] private Gun _gunRight;
    [SerializeField] private Sword _sword;
    [SerializeField] private AnimatorController _animatorController;

    private bool _isGun = false;
    private bool _isSword = true;
    private bool _isFree = true;

    private void Awake()
    {
        _state = BehaviorState.Free;
    }

    public void ChackBehavior()
    {
        switch (_state)
        {
            case BehaviorState.Free:
                BehaviorCondition(false, 0);
                break;

            case BehaviorState.Gun:
                BehaviorCondition(true, 1);
                break;

            case BehaviorState.Sword:
                BehaviorCondition(false, 0);
                break;
        }
       
    }

    public void CheckIsGun()
    {
        if (_isGun)
            _state = BehaviorState.Gun;
        else if (_isSword)
            _state = BehaviorState.Sword;
        else if (_isFree)
            _state = BehaviorState.Free;

        ChackBehavior();

        _isGun = !_isGun;
        _isSword = !_isSword;
        _isFree = !_isFree;
    }

    public void SetIsGun()
    {
        _weightController.SetRigBehavior(0);
        BehaviorCondition(false, 0);
    }

    private void BehaviorCondition(bool condition, float weightCondition)
    {
        _playerInputHendler.SetIsWidthGun(condition);
        _weightController.SetConstrainsBehavior(weightCondition, weightCondition);
        ChangeGunsActivity(condition, condition);
        ChangeSwordActivity(!condition);
    }

    private void ChangeGunsActivity(bool ActivityGunLeft, bool ActivityGunRight)
    {
        _gunLeft.gameObject.SetActive(ActivityGunLeft);
        _gunRight.gameObject.SetActive(ActivityGunRight);
    }

    private void ChangeSwordActivity(bool ActivityGunLeft)
    {
        _sword.gameObject.SetActive(ActivityGunLeft);
    }
}
