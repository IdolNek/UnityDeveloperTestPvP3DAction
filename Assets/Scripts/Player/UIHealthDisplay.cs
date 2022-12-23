using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Color _blinkColor;
    [Range(1,5)][SerializeField] private float _blinkSpeed =3f;
    private Color _startColor = Color.red;
    private bool _isDamaged;
    private float _pinPongLenght = 1f;
    private void OnEnable()
    {
        _health.OnHealthChanged += OnHealthChanged;
    }


    private void OnDisable()
    {
        _health.OnHealthChanged -= OnHealthChanged;
    }
    private void OnHealthChanged(float currentHealth, float maxHealth, float isDamaged—ountdownTime)
    {
        _isDamaged = true;
        _healthBarImage.fillAmount = currentHealth/maxHealth;
        StartCoroutine(DamagedCountdownTime(isDamaged—ountdownTime));
    }
    private void Update()
    {
        if (!_isDamaged) return;
        _healthBarImage.color = Color.Lerp(_startColor, _blinkColor, Mathf.PingPong(Time.time * _blinkSpeed, _pinPongLenght));
    }
    private IEnumerator DamagedCountdownTime(float isDamaged—ountdownTime)
    {
        yield return new WaitForSeconds(isDamaged—ountdownTime);
        _isDamaged = false;
        _healthBarImage.color = _startColor;
    }
}
