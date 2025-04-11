using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookInput : MonoBehaviour
{
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference secondaryAction;
    
    private CinemachineFreeLook _camera;
    private string _x;
    private string _y;

    public CinemachineFreeLook.Orbit[] originalOrbits = new CinemachineFreeLook.Orbit[0];

    [Tooltip("The minimum scale for the orbits")] [Range(0.01f, 1f)]
    public float minScale = 0.5f;

    [Tooltip("The maximum scale for the orbits")] [Range(1F, 5f)]
    public float maxScale = 1;

    [Tooltip("The Vertical axis.  Value is 0..1.  How much to scale the orbits")] [AxisStateProperty]
    public AxisState zAxis = new AxisState(0, 1, false, true, 50f, 0.1f, 0.1f, "Mouse ScrollWheel", false);

    private void OnValidate()
    {
        minScale = Mathf.Max(0.01f, minScale);
        maxScale = Mathf.Max(minScale, maxScale);
    }

    private void Awake()
    {
        _camera = GetComponent<CinemachineFreeLook>();
        
        if (_camera != null && originalOrbits.Length == 0)
        {
            zAxis.Update(Time.deltaTime);
            float scale = Mathf.Lerp(minScale, maxScale, zAxis.Value);
            for (int i = 0; i < Mathf.Min(originalOrbits.Length, _camera.m_Orbits.Length); i++)
            {
                _camera.m_Orbits[i].m_Height = originalOrbits[i].m_Height * scale;
                _camera.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * scale;
            }
        }
    }

    private void Start()
    {
        _x = _camera.m_XAxis.m_InputAxisName;
        _y = _camera.m_YAxis.m_InputAxisName;
        _camera.m_XAxis.m_InputAxisName = "";
        _camera.m_YAxis.m_InputAxisName = "";
        
        _camera.m_XAxis.Value = PlayerPrefs.GetFloat("CameraX");
        _camera.m_YAxis.Value = PlayerPrefs.GetFloat("CameraY");
        _camera.GetComponent<FreeLookInput>().zAxis.Value =  PlayerPrefs.GetFloat("CameraZ");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_camera != null)
        {
            if (originalOrbits.Length != _camera.m_Orbits.Length)
            {
                originalOrbits = new CinemachineFreeLook.Orbit[_camera.m_Orbits.Length];
                Array.Copy(_camera.m_Orbits, originalOrbits, _camera.m_Orbits.Length);
            }

            zAxis.Update(Time.deltaTime);
            float scale = Mathf.Lerp(minScale, maxScale, zAxis.Value);
            for (int i = 0; i < Mathf.Min(originalOrbits.Length, _camera.m_Orbits.Length); i++)
            {
                _camera.m_Orbits[i].m_Height = originalOrbits[i].m_Height * scale;
                _camera.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * scale;
            }

            if (secondaryAction.action.WasPressedThisFrame())
            {
                Vector2 value = look.action.ReadValue<Vector2>();
                _camera.m_XAxis.m_InputAxisValue = value.x;
                _camera.m_YAxis.m_InputAxisValue = value.y;
            }
            else
            {
                _camera.m_XAxis.m_InputAxisValue = 0;
                _camera.m_YAxis.m_InputAxisValue = 0;
            }
        }
    }
}