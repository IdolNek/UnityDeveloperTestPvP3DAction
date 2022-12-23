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
    [SerializeField] private Material _material;
    [SyncVar] private float _currentHeath;
    public event UnityAction<float, float> OnHealthChanged;
    private bool isDamaged = false;

    private void Start()
    {
        _currentHeath = _maxHealth;
    }
    [Server]
    private void SetHealth(float newValue)
    {
        _currentHeath = newValue;
        HealthChanging();
        if (_currentHeath == 0)
        {
            RoundEvent.GetInstance().RemovePlayerNickName(this.GetComponent<NetworkGamePlayer>().NickName);
            if (RoundEvent.GetInstance().IsGameOver())
            {
                RoundEvent.GetInstance().GameOver();
            }
        }
    }
    [Server]
    public void ApplyDamage(float damage)
    {
        if (isDamaged) return;
        isDamaged = true;
        SetHealth(Math.Max(_currentHeath - damage, 0));
        StartCoroutine(Damaged—ountdown());
    }
    private IEnumerator Damaged—ountdown()
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
            _animatorController.SetBool("Death", true);
        }
        else _animatorController.SetTrigger("GetHit");

    }
}
