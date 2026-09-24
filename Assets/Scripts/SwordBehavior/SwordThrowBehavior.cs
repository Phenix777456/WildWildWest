using System;
using UnityEngine;

public class SwordThrowBehavior : StateMachineBehaviour
{
    public event Action ThrowFinished;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ThrowFinished?.Invoke();
    }
}
