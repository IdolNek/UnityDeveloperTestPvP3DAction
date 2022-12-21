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
    [SyncVar] [SerializeField] private float _currentHeath;
    public delegate void HealthChanged(float currentHeath, float maxHealth);
    public event HealthChanged EventHealthChanged;
    //[SyncVar(hook = nameof(SyncHealth))]
    //private float _syncHealth;
    //public void SyncHealth(float oldValue, float newValue)
    //{
    //    _currentHeath = newValue;

    //}
    [Server]
    private void SetHealth(float newValue)
    {
        _currentHeath = newValue;
        OnHealtChanging();
    }
    [Server]
    public void ApplyDamage(float damage)
    {
        SetHealth(Math.Max(_currentHeath - damage, 0));
        Debug.Log("Атака команды Сервера");
    }
    [ClientRpc]
    private void OnHealtChanging()
    {
        EventHealthChanged?.Invoke(_currentHeath, _maxHealth);
    }
    public override void OnStartServer()
    {
        _currentHeath = _maxHealth;
    }
}
