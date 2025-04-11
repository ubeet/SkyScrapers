using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI.Extensions;

public class HeightPickerBehaviour : MonoBehaviour
{
    [SerializeField] private RadialLayout layout;
    [SerializeField] private Transform container;
    [SerializeField] private HeightPickerPresenter prefab;
    [SerializeField] private HeightPickerPresenter zeroPrefab;
    [SerializeField] private PickerController pickerController;
    [SerializeField] private CinemachineInputProviderCustom inputProvider;
    [SerializeField] private RectTransform anchor;
    [SerializeField] private float _openingSpeed;
    [SerializeField] private float _closingSpeed;
    [SerializeField] private CanvasGroup _picker;
    
    
    public static HeightPickerBehaviour Instance;
    private Action<int> _cb;

    
    private PickerState _currentState = PickerState.Closed;
    private float _timer = 0f;
    
    
    private void Awake()
    {
        Instance = this;
        _picker.gameObject.SetActive(false);
        zeroPrefab.Init(0, Return);
        
    }

    private void OnDisable()
    {
        inputProvider.enabled = true;
    }

    public void Init(int range)
    {
        Clear();
        //float tempAngle = 360 - range * (65 - 5 * range);
        //layout.StartAngle = tempAngle;
        switch (range)
        {
            case 0: layout.StartAngle = 360; break;
            case 1: layout.StartAngle = 300; break;
            case 2: layout.StartAngle = 250; break;
            case 3: layout.StartAngle = 210; break;
            case 4: layout.StartAngle = 180; break;
            case 5: layout.StartAngle = 162; break;
            case 6: layout.StartAngle = 149.9f; break;
            case 7: layout.StartAngle = 142; break;
            case 8: layout.StartAngle = 135; break;
        }


        //Debug.Log(-15*(range*range) + 165 * range + 360);
        //layout.StartAngle = -15*(range*range) + 165 * range + 360;
        
        pickerController._pickersArray.Clear();
        for (int i = 1; i <= range; i++)
        {
            var c = Instantiate(prefab, container);
            pickerController._pickersArray.Add(c);
            c.Init(i, Return);
        }
        
    }
 


    public void Show(Vector2 pos, Action<int> cb, List<int> nums)
    {
        anchor.anchoredPosition = pos;
        inputProvider.enabled = false;
        _cb = null;
        _cb = cb;
        gameObject.SetActive(true);
        _currentState = PickerState.Opening;
        foreach (var picker in pickerController._pickersArray)
        {
            picker.CheckColor(nums);
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case PickerState.Opening:
                _timer += _openingSpeed * Time.deltaTime;
                _timer = Mathf.Clamp(_timer, 0, 1);
                _picker.alpha = _timer;
                anchor.localScale = Vector3.one * _timer;
                if (_timer == 1)
                {
                    _currentState = PickerState.Opened;
                }
                break;
            case PickerState.Opened:
                break;
            case PickerState.Closing:
                _timer -= _closingSpeed * Time.deltaTime;
                _timer = Mathf.Clamp(_timer, 0, 1);
                _picker.alpha = _timer;
                anchor.localScale = Vector3.one * _timer;
                if (_timer == 0)
                {
                    _currentState = PickerState.Closed;
                    _picker.gameObject.SetActive(false);
                }
                break;
            case PickerState.Closed:
                break;
        }
    }


    public void Return(int num)
    {
        
        inputProvider.enabled = true;
        _cb?.Invoke(num);
        _currentState = PickerState.Closing;
        
    }

    private void Clear()
    {
        foreach (Transform tr in container)
            Destroy(tr.gameObject);
    }

    private enum PickerState
    {
        Opening,
        Opened,
        Closing,
        Closed
    }


    
}