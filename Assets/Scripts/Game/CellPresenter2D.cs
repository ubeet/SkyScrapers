using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CellPresenter2D : CellPresenter, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rect;
    private bool isSelected;
    private Controls _inputs;
    

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
    
    public void Awake()
    {
        _inputs = new Controls();
        canvas = GameObject.Find ("Canvas").GetComponent <Canvas>();
        _rect = GetComponent<RectTransform>();
    }
    public override void Reset()
    {
        text.text = "";
    }
    
    public override void Set(List<int> dataRuleCell, bool force)
    {
        _heights = dataRuleCell;
        if(dataRuleCell.Count <= 1)
        {
            cellGridPresenter.Clear();
            cellGridPresenter.gameObject.SetActive(false);
            text.gameObject.SetActive(true);
            text.text = dataRuleCell.Count == 0 ? "" : dataRuleCell[0].ToString();
        }
        else
        {
            cellGridPresenter.gameObject.SetActive(true);
            text.gameObject.SetActive(false);
            cellGridPresenter.SetNumbers(dataRuleCell);
        }
            
    }

    public void OnMouseDown()
    {
        if (!Mouse.current.leftButton.isPressed || !_isChangeable) return;
        var position = _rect.position;
        var scaleFactor = canvas.scaleFactor;
        
        HeightPickerBehaviour.Instance.Show(new Vector2(position.x / scaleFactor, position.y / scaleFactor), SetNew, _heights);
    }
    
    private void Update()
    {
        
        if (isSelected && _isChangeable)
        {
            if(_inputs.Player.Nums.WasPressedThisFrame())
            {
                int num = Convert.ToInt32(_inputs.Player.Nums.ReadValue<float>());
                Debug.Log(_index);
                if (num >= 0 && num <= _size)
                    SetNew(num);
                
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                SetNew(0);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
    }
}