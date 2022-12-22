using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class AttackState : NetworkBehaviour
{
    [SerializeField] private float _attackForce;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackDamage;
    private Rigidbody _rigidbody;
    private Vector3 _attackStartPoint;
    private bool _isAttack;
    public bool IsAttack => _isAttack;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();       
    }
    private void FixedUpdate()
    {
        if (!_isAttack) return;
        Vector3 moveVector =  transform.forward * _attackForce * Time.fixedDeltaTime;
        _rigidbody.velocity = moveVector;
        Vector3 currentDistanceAttack = transform.position - _attackStartPoint;
        if (currentDistanceAttack.magnitude >= _attackDistance)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _isAttack = false;
        }

    }
    [Server]
    private void DealDamage(Health health)
    {
        health.ApplyDamage(_attackDamage);
    }
    [Command]
    private void CmdDealDamage(Health health)
    {
        health.ApplyDamage(_attackDamage);
    }
    public void StartAttack()
    {
        _isAttack = true;
        _attackStartPoint = transform.position;
    }
    [ClientCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Health health) && _isAttack)
        {
            if (isServer) DealDamage(health);
            CmdDealDamage(health);
        }
        if (collision.gameObject.layer != 6) _isAttack = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    private void OnCollisionExit(Collision collision)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}

