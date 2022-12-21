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
    [SyncVar] [SerializeField] private float _currentHeath;
    public delegate void HealthChanged(float currentHeath, float maxHealth);
    public event HealthChanged EventHealthChanged;
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
        OnHealtChanging();
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
    private void OnHealtChanging()
    {
        EventHealthChanged?.Invoke(_currentHeath, _maxHealth);
    }
    //public override void OnStartServer()
    //{
    //    SetHealth(_maxHealth);
    //}
}
