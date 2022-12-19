using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceFromTarget;
    [SerializeField] private float _smoothTime;
    [SerializeField] private Vector2 _rotationXMinMax;
    private float _rotationX;
    private float _rotationY;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private void Start()
    {
        this.transform.parent = null;
    }
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _rotationY += mouseX;
        _rotationX += mouseY;
        _rotationX = Math.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);
        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;
        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }
}
