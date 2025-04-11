using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePresenterSetup
{
    public GameObject container;
    public List<Transform> RulesContainers;
    public List<Transform> CellsContainers;
    public CellPresenter CellPresenterPrefab;
    public RulePresenter RulePresenterPrefab;
    public int Width;
    public int Height;
}