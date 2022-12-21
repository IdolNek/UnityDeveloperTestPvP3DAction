using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _isDamaged—ountdownTime;
    [SerializeField] private Animator _animatorController;
    [SyncVar] [SerializeField] private float _currentHeath;
    public delegate void HealthChanged(float currentHeath, float maxHealth);
    public event UnityAction<float, float> OnHealthChanged;
    public event UnityAction OnPlayerDayed;
    private bool isDamaged = false;
    //[SyncVar(hook = nameof(SyncHealth))]
    //private float _syncHealth;
    //public void SyncHealth(float oldValue, float newValue)
    //{
    //    _currentHeath = newValue;

    //}
    private void Start()
    {
        _currentHeath = _maxHealth;
    }
    [Server]
    private void SetHealth(float newValue)
    {
        _currentHeath = newValue;
        HealthChanging();
    }
    [Server]
    public void ApplyDamage(float damage)
    {
        if (isDamaged) return;
        isDamaged = true;
        SetHealth(Math.Max(_currentHeath - damage, 0));
        StartCoroutine(—ountdownIsDamaged());
    }

    IEnumerator —ountdownIsDamaged()
    {
        yield return new WaitForSeconds(_isDamaged—ountdownTime);
        isDamaged = false;
    }

    [ClientRpc]
    private void HealthChanging()
    {
        OnHealthChanged?.Invoke(_currentHeath, _maxHealth);
        if (_currentHeath == 0)
        {
            OnPlayerDayed?.Invoke();
            _animatorController.SetBool("Death", true);
        }
        else _animatorController.SetTrigger("GetHit");

    }
    //public override void OnStartServer()
    //{
    //    SetHealth(_maxHealth);
    //}
}
