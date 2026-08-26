using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwordComboStateBehavior : StateMachineBehaviour 
{
    public event Action StrikeFinished;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StrikeFinished?.Invoke();
    }
}
