using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TimerPresenter : MonoBehaviour
{
    [SerializeField] public TMP_Text timeText;

    public float timeHasPassed = 0;
    private bool _isStopped;

    private void Start()
    {
        _isStopped = false;
    }

    public void StopTime()
    {
        _isStopped = true;
    }
    public void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); 
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = (int)(time % 1 * 100);
        timeText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void TimeCounter(ref float timeLeft)
    {
        if(_isStopped) return;
        UpdateTimerText(timeHasPassed);
        timeHasPassed += Time.deltaTime;
    }

    public void Timer(ref float timeLeft)
    {
        if(_isStopped) return;
        UpdateTimerText(timeLeft);
        timeLeft -= Time.deltaTime;
    }

    public void SetZeroTime()
    {
        timeHasPassed = 0;
    }
}