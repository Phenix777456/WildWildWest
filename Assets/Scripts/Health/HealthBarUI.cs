using System.Collections;
using UnityEngine;


[RequireComponent(typeof(HealthController))]
public class HealthBarUI : MonoBehaviour
{
    [Header("Ссылка на дочерний спрайт-заливку (pivot слева)")]
    [SerializeField] private Transform _fillTransform;

    [Header("Настройки")]
    [SerializeField] private float _fillSpeed = 3f;
    private HealthController _healthController;
    private Coroutine _fillRoutine;
    private float _fullWidth;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
        _fullWidth = _fillTransform.localScale.x;
    }

    private void OnEnable()
    {
        _healthController.OnHealthChanged += HandleHealthChanged;
        SetFillInstant(_healthController.GetFillRatio());
    }

    private void Start()
    {
        SetFillInstant(_healthController.GetFillRatio());
    }

    private void OnDisable()
    {
        _healthController.OnHealthChanged -= HandleHealthChanged;

        if (_fillRoutine != null)
        {
            StopCoroutine(_fillRoutine);
            _fillRoutine = null;
        }
    }

    private void HandleHealthChanged(float targetFillRatio)
    {
        if (_fillRoutine != null)
        {
            StopCoroutine(_fillRoutine);
        }

        _fillRoutine = StartCoroutine(AnimateFill(targetFillRatio));
    }

    private void SetFillInstant(float fillRatio)
    {
        Vector3 scale = _fillTransform.localScale;
        scale.x = _fullWidth * fillRatio;
        _fillTransform.localScale = scale;
    }

    private IEnumerator AnimateFill(float targetFillRatio)
    {
        float targetWidth = _fullWidth * targetFillRatio;

        while (Mathf.Approximately(_fillTransform.localScale.x, targetWidth) == false)
        {
            Vector3 scale = _fillTransform.localScale;
            scale.x = Mathf.MoveTowards(scale.x, targetWidth, _fullWidth * _fillSpeed * Time.deltaTime);
            _fillTransform.localScale = scale;

            yield return null;
        }

        Vector3 finalScale = _fillTransform.localScale;
        finalScale.x = targetWidth;
        _fillTransform.localScale = finalScale;

        _fillRoutine = null;
    }
}
