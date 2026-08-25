using UnityEngine;

public class WeaponAimRigDriver : MonoBehaviour
{
    [SerializeField] private AimTargetProvider _aimTargetProvider;

    [Header("Общая цель для Multi-Aim (торс)")]
    [SerializeField] private Transform _spineAimTarget;

    [Header("Индивидуальные смещения рук относительно оружия")]
    [SerializeField] private Transform _rightHandIkTarget;
    [SerializeField] private Transform _leftHandIkTarget;

    private void LateUpdate()
    {
        Vector3 targetPoint = _aimTargetProvider.CurrentAimPoint;

        ApplySpineAim(targetPoint);
    }

    private void ApplySpineAim(Vector3 worldPoint)
    {
        _spineAimTarget.position = worldPoint;
    }
}