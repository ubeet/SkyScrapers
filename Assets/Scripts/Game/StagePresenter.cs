using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StagePresenter : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Button button;
    [SerializeField] private float startScale;
    [SerializeField] private float onMousePlusScale;
    [SerializeField] private float scalingSpeed = 2f;
    [SerializeField] private TMP_Text sizeText;

    private Transform _currentBuilding;
    private bool _isRotate;
    private Vector3 _targetScale;
    private Vector3 _startScale;
    private Stage _currentStage;
    public void Init(Stage el)
    {
        _targetScale = new Vector3(startScale + onMousePlusScale, startScale + onMousePlusScale, startScale + onMousePlusScale);
        _startScale = new Vector3(startScale, startScale, startScale);
        sizeText.text = el.Height + "x" + el.Width;
        _currentStage = el;
        _currentBuilding = Instantiate(el.stagePrefab, container);
        button.onClick.AddListener(StartStage);
    }

    private void StartStage()
    {
        RootData.Instance.CurrentStage = _currentStage;
        FindObjectOfType<SceneFader>().FadeToScene(1);

    }

    public void IsRotate(bool isRotate)
    {
        _isRotate = isRotate;
    }

    private void Update()
    {
        if(_currentBuilding == null) return;
        _currentBuilding.Rotate(0f, 30f * Time.deltaTime, 0f);
        if (_isRotate)
        {
            if (Vector3.Distance(_currentBuilding.localScale, _targetScale) > 0.01f)
                _currentBuilding.localScale = Vector3.Lerp(_currentBuilding.localScale, _targetScale, Time.deltaTime * scalingSpeed);
        }
        else
        {
            if (Vector3.Distance(_currentBuilding.localScale, _startScale) > 0.01f)
                _currentBuilding.localScale = Vector3.Lerp(_currentBuilding.localScale, _startScale, Time.deltaTime * scalingSpeed);
        }
    }
}