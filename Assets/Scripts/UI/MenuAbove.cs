using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuAbove : MonoBehaviour
{
    private float _alpha = 0f;
    private CanvasGroup _canvasGroup;
    private bool _isTransitionIn;
    private bool _isTransitionOut;

    private void Awake()
    {
        _canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void OpenMenuAbove()
    {
        gameObject.SetActive(true);
        _isTransitionIn = true;
        _isTransitionOut = false;
    }
    
    public void CloseMenuAbove()
    {
        _isTransitionOut = true;
        _isTransitionIn = false;
    }


    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void Update()
    {
        if (_isTransitionIn)
        {
            _alpha += 9 * Time.deltaTime;
            _canvasGroup.alpha = _alpha;
            if (_alpha >= 1f)
            {
                _alpha = 1;
                _isTransitionIn = false;
            }
        }
        if (_isTransitionOut)
        {
            _alpha -= 9 * Time.deltaTime;
            _canvasGroup.alpha = _alpha;
            if (_alpha <= 0f)
            {
                _alpha = 0;
                _isTransitionOut = false;
                gameObject.SetActive(false);
            }
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseMenuAbove();
    }
}