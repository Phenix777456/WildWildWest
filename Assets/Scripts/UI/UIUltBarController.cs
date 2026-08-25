
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIUltBarController : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private float _duration = 0.5f;

    public void ChangeHpBar(float points)
    {
        float fillPonts = points / 100;
        StartCoroutine(FillHpBar(fillPonts));
    }

    private IEnumerator FillHpBar(float points)
    {
        float startValue = _hpBar.fillAmount;
        float targetValue = startValue + points;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            _hpBar.fillAmount = Mathf.Lerp(startValue, targetValue, elapsed / _duration);
            yield return null;
        }

        _hpBar.fillAmount = targetValue;
    }

    public float CheckHPFill()
    {
        return _hpBar.fillAmount;
    }
}
