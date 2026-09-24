using System;
using UnityEngine;

public interface IDamageable
{
    event Action Died;
}

public class HealthController : MonoBehaviour , IDamageable
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action Died;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _currentHealth <= 0f;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public float GetFillRatio() => _currentHealth / _maxHealth;

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || IsDead)
        {
            return;
        }

        SetHealth(_currentHealth - amount);

        if (IsDead)
        {
            Died?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f || IsDead)
        {
            return;
        }

        SetHealth(_currentHealth + amount);
    }

    private void SetHealth(float newValue)
    {
        _currentHealth = Mathf.Clamp(newValue, 0f, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth / _maxHealth);
    }
}
