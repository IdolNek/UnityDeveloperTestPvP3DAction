using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceFromTarget;
    [SerializeField] private float _smoothTime;
    [SerializeField] private Vector2 _rotationXMinMax;
    private Camera _mainCamera;
    private float _rotationX;
    private float _rotationY;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    public override void OnStartLocalPlayer()
    {
        if (_mainCamera != null)
        {
            // configure and make camera a child of player with 3rd person offset
            _mainCamera.orthographic = false;
            _mainCamera.transform.SetParent(transform);
        }
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
        _mainCamera.transform.localEulerAngles = _currentRotation;
        _mainCamera.transform.position = _target.position - transform.forward * _distanceFromTarget;
    } 
    public override void OnStopLocalPlayer()
    {
        if (_mainCamera != null)
        {
            _mainCamera.orthographic = true;
            SceneManager.MoveGameObjectToScene(_mainCamera.gameObject, SceneManager.GetActiveScene());
            _mainCamera.transform.localPosition = new Vector3(0f, 70f, 0f);
            _mainCamera.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }
}
