using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private AttackState _attackState;
    private Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void MovePlayer(Vector3 moveDirection)
    {
        if (_attackState.IsAttack) return;
        if (moveDirection != Vector3.zero)
        {
            Quaternion desireRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desireRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
        Vector3 moveVector = moveDirection * _moveSpeed * Time.fixedDeltaTime;
        _rigidbody.velocity = new Vector3(moveVector.x, _rigidbody.velocity.y, moveVector.z);
    }
    public void Attack()
    {
        _attackState.StartAttack();
    }
}
