using UnityEngine;


[RequireComponent(typeof(WeaponTrigger))]
public class WeaponDamageDealer : MonoBehaviour
{
    public bool IsAttacking { get; private set; } = true;
    //Логика изменения данной константы для избежания получение урона вне атаки будет добавлена позже

    private WeaponTrigger _weaponTrigger;

    private void Awake()
    {
        _weaponTrigger = GetComponent<WeaponTrigger>();
    }

    private void OnEnable()
    {
        _weaponTrigger.OnHit += HandleWeaponHit;
    }

    private void OnDisable()
    {
        _weaponTrigger.OnHit -= HandleWeaponHit;
    }

    private void HandleWeaponHit(float damage, Collider targetCollider)
    {
        if (targetCollider.TryGetComponent(out HealthController health))
        {
            if (IsAttacking)
                health.TakeDamage(damage);
        }
    }
}
