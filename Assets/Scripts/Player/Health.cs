using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private float _maxHealth;
    [SyncVar] private float _currentHeath;
    public delegate void HealthChanged(float currentHeath, float maxHealth);
    public event Action<float,float> EventHealthChanged;
    [Server]
    private void SetHealth(float value)
    {
        _currentHeath = value;
        EventHealthChanged?.Invoke(_currentHeath, _maxHealth);
    }
    public override void OnStartServer()
    {
        SetHealth(_maxHealth);
    }
    [Command]
    public void CmdDealDamage(float damage)
    {
        SetHealth(Mathf.Max(_currentHeath - damage,0));
    }
}
