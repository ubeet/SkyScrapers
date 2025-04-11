using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ResultResolverBase : MonoBehaviour
{
    [SerializeField] private ResultsScreens resultsScreens;
    [SerializeField] private CinemachineFreeLook FreeLook;
    [SerializeField] private CinemachineInputProviderCustom InputProvider;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float zSpeed;
    [SerializeField] private List<CanvasGroup> ToDisable;
    [SerializeField] private TimerPresenter TimerPresenter;
    [SerializeField] private FireworkPresenter FireworkPresenter;

    [SerializeField] private float alphaHideStep = 0.1f;
        
    [NonSerialized] public bool _onGameEndAnimation;
    private bool _canZoom;
    private Exposure _exposure;
    private float _yvel;
    private float _zvel;
    private float _alpha;


    private void Start()
    {
        _alpha = 1;
        if (_exposure != null)
            _exposure.fixedExposure.value = 9.7f;
        _canZoom = true;
        _onGameEndAnimation = false;
    }

    public void Win()
    {
        InputProvider.Block();
        TimerPresenter.StopTime();
        FireworkPresenter.Play();
        HideIn();
        resultsScreens.SetWinStateScreen(true);
    }

    private void HideIn()
    {
        _onGameEndAnimation = true;
        foreach (var el in ToDisable)
        {
            el.interactable = false;
            el.blocksRaycasts = false;
        }
    }

    public void Lose()
    {
        InputProvider.Block();
        TimerPresenter.StopTime();
        HideIn();
        resultsScreens.SetWinStateScreen(false);
    }

    private void Update()
    {
        #if UNITY_EDITOR
            if (Keyboard.current.f9Key.isPressed)
            {
                Win();
            }
            if (Keyboard.current.f10Key.isPressed)
            {
                Lose();
            }
        #endif
        
        if (_onGameEndAnimation)
        {
            _alpha -= alphaHideStep;
            
            if (_alpha >= -alphaHideStep)
                foreach (var el in ToDisable)
                {
                    //el.gameObject.SetActive(false);
                    el.alpha = _alpha;
                }
            
            // var fixedValue = _exposure.fixedExposure.value;
            // if (_exposure != null && fixedValue < 12f)
            //     _exposure.fixedExposure.value += 0.005f;
            
            FreeLook.m_XAxis.Value += xSpeed;
            FreeLook.m_YAxis.Value = Mathf.SmoothDamp(FreeLook.m_YAxis.Value, ySpeed, ref _yvel, 1, 200);
            FreeLook.m_Lens.FieldOfView = Mathf.SmoothDamp(FreeLook.m_Lens.FieldOfView, zSpeed, ref _zvel, 1, 200);
        }
    }
}