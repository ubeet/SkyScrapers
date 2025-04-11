using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RulePresenter3D : RulePresenter
{
    [SerializeField] private Transform canvas;

    private Transform camera;

    private void Awake()
    {
        camera = Camera.main.transform;
    }
    private void Update()
    {
        canvas.rotation = Quaternion.Euler(90, 0, -camera.rotation.eulerAngles.y);
    }

    public override void Set(int dataRuleCell, bool force)
    {
        base.Set(dataRuleCell, force);
        text.text = dataRuleCell == 0 ? "" : dataRuleCell.ToString();
    }

    public override void Reset()
    {
        text.text = "";
    }
}