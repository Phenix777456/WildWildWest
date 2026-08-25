using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeightController : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint _leftHend;
    [SerializeField] private TwoBoneIKConstraint _rightHend;
    [SerializeField] private Rig _rig;

    public void SetConstrainsBehavior(float constrain1, float constrain2)
    {
        _leftHend.weight = constrain1;
        _rightHend.weight = constrain2;
    }


    public void SetRigBehavior(float rig)
    {
        _rig.weight = rig;
    }

    
}
