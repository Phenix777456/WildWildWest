using System;
using UnityEngine;

public class SwordAppearStateBehaviour : StateMachineBehaviour
{
    public event Action AppearFinished;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AppearFinished?.Invoke();
    }
}