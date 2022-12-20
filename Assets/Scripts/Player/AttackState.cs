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
    private float _raycastDistance = 0.5f;
    [SyncVar]
    private bool _isAttack;
    //[SyncVar(hook = nameof(SyncIsAttacking))]
    //private bool _syncIsAttack;
    public bool IsAttack => _isAttack;
    //private void SyncIsAttacking(bool oldValue, bool newValue)
    //{
    //    _isAttacking = newValue;
    //}
    //[Server]
    //private void ChangeIsAttacking(bool newValue)
    //{
    //    _syncIsAttack = newValue;
    //}
    //[Command]
    //private void CmdChangeIsAttacking(bool newValue)
    //{
    //    ChangeIsAttacking(newValue);
    //}

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();       
    }
    private void FixedUpdate()
    {
        if (!_isAttack) return;
        TryToDealDamage();
        Vector3 moveVector =  transform.forward * _attackForce * Time.fixedDeltaTime;
        _rigidbody.velocity = moveVector;
        Vector3 currentDistanceAttack = transform.position - _attackStartPoint;
        if (currentDistanceAttack.magnitude >= _attackDistance)
        {
            _isAttack = false;
            _rigidbody.velocity = Vector3.zero;
        }
    }
    [ClientRpc]
    private void TryToDealDamage()
    {
        bool isHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _raycastDistance);
        Debug.Log($"{_isAttack} - атака");
        Debug.Log($"{ isHit} - изхит");
        if(isHit) Debug.Log($"{hit.collider.gameObject.TryGetComponent(out Health pop)} - есть у него жизни");
        if (isHit && hit.collider.gameObject.TryGetComponent(out Health health) && _isAttack)
        {
            Debug.Log("Прошла атака по ХИТУ");
            if(isServer) health.DealDamagege(_attackDamage);
            else health.CmdDealDamage(_attackDamage);
            _isAttack = false;
        }
    }
    public void StartAttack()
    {
        _isAttack = true;
        _attackStartPoint = transform.position;
    }
    //[ClientCallback]
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Коллизия сработала");
    //    Debug.Log(_isAttack);
    //    Debug.Log(collision.gameObject.name);
    //    Debug.Log(collision.gameObject.TryGetComponent(out Health pop));
    //    if (collision.gameObject.TryGetComponent(out Health health) && _isAttack)
    //    {
    //            health.DealDamagege(_attackDamage);
    //            health.CmdDealDamage(_attackDamage);
    //    }
    //    _isAttack = false;
    //}
    //private void IsAttackingChanged(bool value)
    //{
    //    if (isClient) CmdChangeIsAttacking(value);
    //    else ChangeIsAttacking(value);
    //}
}

