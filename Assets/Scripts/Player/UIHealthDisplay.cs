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
        _health.EventHealthChanged += OnHealthChanged;
    }


    private void OnDisable()
    {
        _health.EventHealthChanged -= OnHealthChanged;
    }
    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        Debug.Log("мен€ю жизни в баре");
        _healthBarImage.fillAmount = currentHealth/maxHealth;
    }
}
