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
    private bool _isAttack;
    public bool IsAttack => _isAttack;
    //[SyncVar(hook = nameof(SyncIsAttacking))]
    //private bool _syncIsAttack;
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
        // TryToDealDamage();
        Vector3 moveVector =  transform.forward * _attackForce * Time.fixedDeltaTime;
        _rigidbody.velocity = moveVector;
        Vector3 currentDistanceAttack = transform.position - _attackStartPoint;
        if (currentDistanceAttack.magnitude >= _attackDistance)
        {
            _isAttack = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

    }
    private void TryToDealDamage()
    {
        bool isHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _raycastDistance);
        if (isHit && hit.collider.gameObject.TryGetComponent(out Health health) && _isAttack)
        {
            Debug.Log("���� �� ����");
            _isAttack = false;
            if (isServer) DealDamage (health);
            CmdDealDamage(health);
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
    private void OnCollisionExit(Collision collision)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out Health health) && _isAttack)
    //    {
    //        health.GetComponent<NetworkIdentity>().RemoveClientAuthority();
    //    }
    //}
    //[Command]
    //private void TryToDealDamge(GameObject collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out Health health) && _isAttack)
    //    {
    //        // health.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    //        health.DealDamagege(_attackDamage);
    //        health.CmdDealDamage(_attackDamage);
    //    }
    //}

}

