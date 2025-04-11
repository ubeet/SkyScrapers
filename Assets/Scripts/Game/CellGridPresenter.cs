using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CellGridPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text textPrefab;
    [SerializeField] private bool is2d;
    private List<TMP_Text> _filledCells;
    private GridLayoutGroup _gridLayoutGroup;
    private Vector2 _size;
    private float _gridSize;
    private Camera _main;
    
    
    public void Init(int size, float gridSize)
    {
        _main = Camera.main;
        _filledCells = new List<TMP_Text>(new TMP_Text[size]);
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        var sqrt = Mathf.Sqrt(size);
        int w;
        int h;
        if(size == 4)
        {
            w = (int) sqrt;
            h = (int) sqrt;
        }
        else
        {
            w = 3;
            h = Mathf.CeilToInt(size / 3f);
        }

        if (gridSize == 0)
            gridSize = 2f;
        
        var cellWidth = gridSize / (float)w;
        var cellHeight = gridSize / (float)h;

        _gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        for (int i = 0; i < size; i++)
        {
            var el = Instantiate(textPrefab, _gridLayoutGroup.transform);
            el.text = "";
            _filledCells[i] = el;
        }

        transform.localScale = new Vector3(1, 1, 1);
    }
    

    public void SetNumber(int num)
    {
        if(num < 1) return;
        _filledCells[num - 1].text = num.ToString();
    }

    public void Clear()
    {
        foreach (var el in _filledCells)
        {
            el.text = "";
        }
    }

    public void SetNumbers(List<int> dataRuleCell)
    {
        Clear();
        foreach (var el in dataRuleCell)
        {
            SetNumber(el);
        }
    }

    private void Update()
    {
        if(is2d) return;
        transform.rotation = Camera.main.transform.rotation;
    }
}