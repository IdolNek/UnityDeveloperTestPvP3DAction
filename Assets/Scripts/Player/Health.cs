using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Health : NetworkBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _isDamaged—ountdownTime;
    [SerializeField] private Animator _animatorController;
    [SyncVar] private float _currentHeath;
    private bool _isDamaged = false;
    private bool _isDead = false;
    public bool IsDead => _isDead;
    public event UnityAction<float, float,float> OnHealthChanged;

    private void Start()
    {
        _currentHeath = _maxHealth;
        RoundEvent.GetInstance().OnStartGame += OnStartGame;
    }
    private void OnDisable()
    {
        RoundEvent.GetInstance().OnStartGame -= OnStartGame;
    }
    private void OnStartGame(Transform[] spownPoints)
    {
        SetHealth(_maxHealth);
        _isDead = false;
        int spownPoint = Random.Range(0, spownPoints.Length);
        transform.position = spownPoints[spownPoint].position;
        _animatorController.SetBool("Death", false);
    }
    [Server]
    private void SetHealth(float newValue)
    {
        _currentHeath = newValue;
        HealthChanging();
        if (_currentHeath == 0)
        {
            RoundEvent.GetInstance().RemovePlayerNickName(this.GetComponent<NetworkGamePlayer>().NickName);
            Debug.Log($"»„‡ Á‡ÍÓÌ˜ÂÌ‡ - {RoundEvent.GetInstance().IsGameOver()}");
            if (RoundEvent.GetInstance().IsGameOver())
            {
                //this.GetComponent<NetworkGamePlayer>().Score += 1;
                RoundEvent.GetInstance().GameOver();
            }
        }
    }
    [Server]
    public void ApplyDamage(float damage)
    {
        if (_isDamaged) return;
        _isDamaged = true;
        SetHealth(Math.Max(_currentHeath - damage, 0));
        StartCoroutine(Damaged—ountdown());
    }
    private IEnumerator Damaged—ountdown()
    {
        yield return new WaitForSeconds(_isDamaged—ountdownTime);
        _isDamaged = false;
    }

    [ClientRpc]
    private void HealthChanging()
    {
        OnHealthChanged?.Invoke(_currentHeath, _maxHealth, _isDamaged—ountdownTime);
        if (_currentHeath == 0)
        {
            _isDead = true;
            _animatorController.SetBool("Death", true);
        }
        else _animatorController.SetTrigger("GetHit");

    }
}
