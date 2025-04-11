using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PickerController : MonoBehaviour
{
    [SerializeField] private GameObject bodyContainer;
    [SerializeField] private HeightPickerPresenter zeroPicker;
    [SerializeField] private HeightPickerBehaviour heightPickerBehaviour;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float inputDeadZone = 40f;
    [SerializeField] private float pickLerpDuration = 0.05f;
    [SerializeField] private Color pickerPickImageColor = new Color(0.47f, 0.57f, 0.82f);
    [SerializeField] private Color pickerUnPickImageColor = new Color(0.18f, 0.18f, 0.18f);
    
        
    private Controls _inputs;
    private Vector2 _direction;
    [NonSerialized] public List<HeightPickerPresenter> _pickersArray = new List<HeightPickerPresenter>();
    private HeightPickerPresenter _picker;

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        if(_picker != null)
            _picker.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
        StopAllCoroutines();
        _inputs.Disable();
    }

    public void ReturnPicker(bool isGrid)
    {
        CurrentPicker();
        _picker.Return(isGrid);
    }

    private void CurrentPicker()
    {
        var distance = Vector2.Distance(_rect.position, Mouse.current.position.ReadValue());
        if (distance > inputDeadZone)
        {
            _rect.gameObject.SetActive(true);
            _picker = GetPicker(_direction);
        }
        else
        {
            _rect.gameObject.SetActive(false);
            _picker = zeroPicker;
        }
    }

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            heightPickerBehaviour.gameObject.SetActive(false);
            return;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            
            if (Keyboard.current.shiftKey.isPressed)
            {
                ReturnPicker(true);
            }
            else
            {
                ReturnPicker(false);
            }
        }
    }


    private HeightPickerPresenter GetPicker(Vector2 vec)
    {
        float pickerAngle = 360f / _pickersArray.Count / 2f;
        for (var i = 0; i < _pickersArray.Count; i++)
        {
            
            float angleBetweenVectors = Vector2.Angle(_pickersArray[i].transform.localPosition, vec);
            if (angleBetweenVectors <= pickerAngle || 360f - angleBetweenVectors <= pickerAngle)
                return _pickersArray[i];
        }

        return zeroPicker;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _picker = zeroPicker;
    }
    
    private void Awake()
    {
        _inputs = new Controls();
    }

    IEnumerator Pick(HeightPickerPresenter picker)
    {
        float timeElapsed = 0;
        RectTransform rectPicker = picker.GetComponent<RectTransform>();
        while (timeElapsed < pickLerpDuration)
        {
            float lerpSize = Mathf.Lerp(0.7f, 0.9f, timeElapsed / pickLerpDuration);
            Color lerpColor = Color.Lerp(pickerUnPickImageColor, pickerPickImageColor,
                timeElapsed / pickLerpDuration);

            picker._image.color = lerpColor;

            rectPicker.localScale = new Vector3(lerpSize, lerpSize, 1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        picker._image.color = pickerPickImageColor;
    }

    IEnumerator UnPick(HeightPickerPresenter picker)
    {
        float timeElapsed = 0;
        float lerpDuration = 0.05f;
        RectTransform rectPicker = picker.GetComponent<RectTransform>();
        while (timeElapsed < lerpDuration)
        {
            float lerpSize = Mathf.Lerp(0.9f, 0.7f, timeElapsed / lerpDuration);
            Color lerpColor = Color.Lerp(pickerPickImageColor, pickerUnPickImageColor,
                timeElapsed / pickLerpDuration);

            picker._image.color = lerpColor;

            rectPicker.localScale = new Vector3(lerpSize, lerpSize, 1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        picker._image.color = pickerUnPickImageColor; 
    }

    IEnumerator Choose()
    {
        float timeElapsed = 0;
        float lerpDuration = 0.1f;
        var startRot = _rect.rotation;
        bool isMinus = false;
        float angle = (_direction.x > 0
            ? -Vector2.Angle(new Vector2(0, 1),
                new Vector2(_picker.transform.localPosition.x, _picker.transform.localPosition.y))
            : Vector2.Angle(new Vector2(0, 1),
                new Vector2(_picker.transform.localPosition.x, _picker.transform.localPosition.y))) + 45f;
        
        while (timeElapsed < lerpDuration)
        {
            Quaternion lerpRot = Quaternion.Lerp(startRot, Quaternion.Euler(0, 0, angle), timeElapsed / lerpDuration);

            _rect.rotation = lerpRot;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _rect.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        if(_pickersArray.Count == 0) return;
        var pc = _picker;
        CurrentPicker();
        _direction = Mouse.current.position.ReadValue() - (Vector2) _rect.position;
        if (pc == _picker) return;
        if(pc != null)
            StartCoroutine(UnPick(pc));
        StartCoroutine(Pick(_picker));
        StartCoroutine(Choose());
    }
}