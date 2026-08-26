using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeightController : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint _leftHend;
    [SerializeField] private TwoBoneIKConstraint _rightHend;
    [SerializeField] private Rig _rig;
    [SerializeField] private float _rigWeightStep;

    private Coroutine _rigWeightCoroutine;

    public void SetConstrainsBehavior(float constrain1, float constrain2)
    {
        _leftHend.weight = constrain1;
        _rightHend.weight = constrain2;
    }


    public void SetRigBehavior(float targetWeight)
    {
        targetWeight = Mathf.Clamp01(targetWeight);

        if (_rigWeightCoroutine != null)
            StopCoroutine(_rigWeightCoroutine);

        _rigWeightCoroutine = StartCoroutine(ChangeRigWeight(targetWeight));
    }

    private IEnumerator ChangeRigWeight(float targetWeight)
    {
        while (!Mathf.Approximately(_rig.weight, targetWeight))
        {
            _rig.weight = Mathf.MoveTowards(
                _rig.weight,
                targetWeight,
                _rigWeightStep);

            yield return null;
        }

        _rig.weight = targetWeight;
        _rigWeightCoroutine = null;
    }


}
