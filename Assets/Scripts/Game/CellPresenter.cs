using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellPresenter : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public TMP_Text text;
    [SerializeField] public CellGridPresenter cellGridPresenter;
    protected Canvas canvas;
    protected int _index;
    protected Action<int, int> _onChange;
    protected int _size;
    public bool _isChangeable = true;
    private GameMaster _gm;
    
    protected List<int> _heights = new List<int>();

    public virtual void Init(int index, Action<int, int> onChange, int size, GameMaster gm, float gridSize = 0)
    {
        cellGridPresenter.Init(size, gridSize);
        cellGridPresenter.gameObject.SetActive(false);
        _isChangeable = true;
        _size = size;
        _onChange = onChange;
        _index = index;
        _gm = gm;
    }
    private void Awake()
    {
        Reset();
    }

    public virtual void Reset()
    {
    }

    public virtual void Set(List<int> dataGameCell, bool force)
    {
        
    }


    protected void SetNew(int selected)
    {
        /*if (selected == 0)
        {
            Achievements.GetStat("destroyed_houses", out int data);
            Achievements.SetStat("destroyed_houses", data + 1);
        }
        else
        {
            Achievements.GetStat("established_houses", out int data);
            Achievements.SetStat("established_houses", data + 1);
        }*/
        _onChange?.Invoke(_index, selected);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        
    }

}