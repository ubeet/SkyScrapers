using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePresenter2D : GamePresenter
{
    [SerializeField] protected GridLayoutGroup container;
    [SerializeField] protected RectTransform ContainerPrefab;
    public override void Init(GameMaster gameMaster)
    {
        DestroyAllElements(_currentSetup.RulesContainers);
        DestroyAllElements(_currentSetup.CellsContainers);
        DestroyAllElements(_ruleCells);
        DestroyAllElements(_gameCells);
        DestroyAllElementsInChildren(container.gameObject);
        _master = gameMaster;
        gameMaster.OnChanged += OnChanged;
        _currentSetup.Width = _master.GameData.Width;
        _currentSetup.Height = _master.GameData.Height;
        CreateCells();
        var vec = new Vector2(400 / (float)(_currentSetup.Width + 2), 400 / (float)(_currentSetup.Width + 2));
        container.cellSize = vec;
        InitSetup(container.cellSize.x);
    }
    private void DestroyAllElements(List<Transform> toDestroy)
    {
        if (toDestroy == null) return;
        foreach (var el in toDestroy)
        {
            Destroy(el.gameObject);
        }
        toDestroy.Clear();
    }
    
    private void DestroyAllElements(List<RulePresenter> toDestroy)
    {
        if (toDestroy == null) return;
        foreach (var el in toDestroy)
        {
            Destroy(el.gameObject);
        }
        toDestroy.Clear();
    }
    private void DestroyAllElements(List<CellPresenter> toDestroy)
    {
        if (toDestroy == null) return;
        foreach (var el in toDestroy)
        {
            Destroy(el.gameObject);
        }
        toDestroy.Clear();
    }
    private void DestroyAllElementsInChildren(GameObject toDestroy)
    {
        if (toDestroy == null) return;
        foreach (Transform el in toDestroy.GetComponentInChildren<Transform>())
        {
            Destroy(el.gameObject);
        }
    }
    
    private void CreateCells()
    {
        List<Transform> rulesTop = new List<Transform>();
        List<Transform> rulesRight = new List<Transform>();
        List<Transform> rulesBottom = new List<Transform>();
        List<Transform> rulesLeft = new List<Transform>();
        int heightSize = _currentSetup.Height + 2;
        int widthSize = _currentSetup.Width + 2;
        for (int i = 0; i < widthSize; i++)
        {
            var cell = Instantiate(ContainerPrefab.transform, container.transform);
            if(i > 0 && i < widthSize - 1) rulesTop.Add(cell);
        }
        for (int i = 0; i < heightSize - 2; i++)
        {
            for (int j = 0; j < widthSize; j++)
            {
                var cell = Instantiate(ContainerPrefab.transform, container.transform);
                if (j == 0) rulesLeft.Add(cell);
                else if (j == widthSize - 1) rulesRight.Add(cell);
                else _currentSetup.CellsContainers.Add(cell);
            }
        }
        for (int i = 0; i < widthSize; i++)
        {
            var cell = Instantiate(ContainerPrefab.transform, container.transform);
            if(i > 0 && i < widthSize - 1) rulesBottom.Add(cell);
        }

        rulesBottom.Reverse();
        rulesLeft.Reverse();
        _currentSetup.RulesContainers.AddRange(rulesTop);
        _currentSetup.RulesContainers.AddRange(rulesRight);
        _currentSetup.RulesContainers.AddRange(rulesBottom);
        _currentSetup.RulesContainers.AddRange(rulesLeft);
    }
}