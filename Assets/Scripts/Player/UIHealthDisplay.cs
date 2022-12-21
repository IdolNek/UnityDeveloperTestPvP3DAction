using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarImage;
    private void OnEnable()
    {
        _health.OnHealthChanged += OnHealthChanged;
    }


    private void OnDisable()
    {
        _health.OnHealthChanged -= OnHealthChanged;
    }
    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        _healthBarImage.fillAmount = currentHealth/maxHealth;
    }
}
