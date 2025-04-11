using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeightPickerPresenter : MonoBehaviour
{
    [SerializeField] protected TMP_Text text;
    [SerializeField] public PickerController pickerController;
    [SerializeField] public Image _image;

    public int _num;
    private Action<int> _cb;

    
    public void Init(int number, Action<int> cb)
    {
        _num = number;
        _cb = null;
        _cb = cb;
        if (number != 0)
            text.text = number.ToString();


    }
    public void Return(bool isGrid)
    {
        _cb?.Invoke(_num);
    }

    public void CheckColor(List<int> nums)
    {
        if (nums.Contains(_num))
            text.color = Color.red;
        else
            text.color = Color.white;
    }
}