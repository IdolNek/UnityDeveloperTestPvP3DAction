using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private AttackState _attackState;
    [SerializeField] private Animator _animatorController;
    [SerializeField] private Health _health;
    private bool _isDead = false;
    private Rigidbody _rigidbody;
    private void OnEnable()
    {
        _health.OnPlayerDayed += IsDead;
    }
    private void OnDisable()
    {
        _health.OnPlayerDayed -= IsDead;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void MovePlayer(Vector3 moveDirection)
    {
        if (_isDead) return;
        if (_attackState.IsAttack) return;
        // Debug.Log(moveDirection);
        if (moveDirection != Vector3.zero)
        {
            Quaternion desireRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desireRotation, _rotationSpeed * Time.fixedDeltaTime);
            _animatorController.SetBool("Move", true);
        } else _animatorController.SetBool("Move", false);
            Vector3 moveVector = moveDirection * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.velocity = new Vector3(moveVector.x, _rigidbody.velocity.y, moveVector.z);
    }
    public void Attack()
    {
        if (_isDead) return;
        _attackState.StartAttack();
        _animatorController.SetTrigger("Attack");
    }
    private void IsDead() => _isDead = true;
}
