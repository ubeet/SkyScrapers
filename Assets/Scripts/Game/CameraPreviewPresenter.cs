using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPreviewPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform Image;
    [SerializeField] private CinemachineFreeLook Camera;
    [SerializeField] private GameMaster master;

    private RectTransform _rect;
    private float _cameraHeight;


    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _cameraHeight = -400 / ((float)RootData.Instance.CurrentStage.Width + 2) * (RootData.Instance.CurrentStage.Height + 2) - 50;
    }

    private void Update()
    {
        var pos = Camera.m_XAxis.Value;
        Image.rotation = Quaternion.Euler(0, 0, -(pos-180));
        if (pos > -45 && pos <= 45)
        {
            var vec = Vector2.Lerp(new Vector2(450, _cameraHeight), new Vector2(0, _cameraHeight), (pos + 45) / 90f);
            _rect.anchoredPosition = vec;
        }
        if (pos > 45 && pos <= 135)
        {
            var vec = Vector2.Lerp(new Vector2(0, _cameraHeight), new Vector2(0, 0), (pos - 45) / 90f);
            _rect.anchoredPosition = vec;
        }
        if ((pos > 135 && pos <= 180) || (pos >= -180 && pos <= -135))
        {
            if (pos > 135) pos -= 135;
            else if (pos <= -135) pos = (180 + pos) + 45;
            
            var vec = Vector2.Lerp(new Vector2(0, 0), new Vector2(450, 0), (pos) / 90f);
            _rect.anchoredPosition = vec;
        }
        if (pos > -135 && pos <= -45)
        {
            var vec = Vector2.Lerp(new Vector2(450, 0), new Vector2(450, _cameraHeight), (pos + 135) / 90f);
            _rect.anchoredPosition = vec;
        }
    }
}