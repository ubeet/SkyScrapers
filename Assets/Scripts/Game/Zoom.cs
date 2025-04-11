using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float zoomInMax = 40f;
    [SerializeField] private float zoomOutMax = 90f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float smoothTime = 5f;
    
    private CinemachineInputProviderCustom _inputProvider;
    private CinemachineFreeLook _freeLookCamera;
    private float _zAxis;
    private float _fov;
    private float _target;
    private float _velocity = 0f;

    private void ZoomScreen(float zAxis)
    {
        _fov = _freeLookCamera.m_Lens.FieldOfView;
        _target = Mathf.Clamp(_fov + zAxis * zoomSpeed, zoomInMax, zoomOutMax);
    }
    
    private void Awake()
    {
        _inputProvider = GetComponent<CinemachineInputProviderCustom>();
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
        _target = _freeLookCamera.m_Lens.FieldOfView;
        _zAxis = _inputProvider.GetAxisValue(2);
    }

    private void Update()
    {
        if (_inputProvider._isBlocked) return;
        _fov = _freeLookCamera.m_Lens.FieldOfView;
        _zAxis = _inputProvider.GetAxisValue(2);
        if(_zAxis != 0)
            ZoomScreen(_zAxis);
        _freeLookCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(_fov, _target, ref _velocity, smoothTime, maxSpeed);
    }
}