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
    [SerializeField] private float _currentHeath;
    public delegate void HealthChanged(float currentHeath, float maxHealth);
    public HealthChanged EventHealthChanged;
    [SyncVar(hook = nameof(SyncHealth))]
    private float _syncHealth;
    public void SyncHealth(float oldValue, float newValue)
    {
        _currentHeath = newValue;
        EventHealthChanged?.Invoke(_currentHeath, _maxHealth);
    }
    [Server]
    private void SetHealth(float newValue)
    {
        _syncHealth = newValue;
    }
    [Command]
    public void CmdDealDamage(float newValue)
    {
        SetHealth(Math.Max(_currentHeath - newValue, 0));
        Debug.Log("Атака команды клинета");
    }
    [Server]
    public void ApplyDamage(float newValue)
    {
        SetHealth(Math.Max(_currentHeath - newValue, 0));
        Debug.Log("Атака команды Сервера");
    }
    public override void OnStartServer()
    {
        SetHealth(_maxHealth);
    }
}
