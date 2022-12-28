using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttackState : NetworkBehaviour
{
    [SerializeField] private float _attackForce;
    [SerializeField] private float _attackDistance;
    [SerializeField] private PlayerScore _score;
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
        if (Vector3.Distance(_attackStartPoint, transform.position) >= _attackDistance)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _isAttack = false;
        }

    }
    public void StartAttack()
    {
        _isAttack = true;
        _attackStartPoint = transform.position;
    }
    [ClientCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out InvincibleState invicible) && _isAttack)
        {
            if (invicible.IsDamaged) return;
            if (isServer) DealDamage(invicible);
            else CmdDealDamage(invicible);
            if (isServer) AddScore();
            else CmdAddScore();
        }
        if (collision.gameObject.layer != 6) _isAttack = false;
    }
    [Server]
    private void DealDamage(InvincibleState invicible)
    {
        invicible.ApplyDamage();
    }
    [Command]
    private void CmdDealDamage(InvincibleState invicible)
    {
        invicible.ApplyDamage();
    }
    [Command] 
    private void CmdAddScore()
    {
        AddScore();
    }
    [Server]
    private void AddScore()
    {
        _score.AddScoreForPlayer();
    }
    [ClientCallback]
    private void OnCollisionStay(Collision collision)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    [ClientCallback]
    private void OnCollisionExit(Collision collision)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}

