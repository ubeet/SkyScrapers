using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePresenter3D : GamePresenter
{
    [SerializeField] private Transform ContainerPrefab;
    [SerializeField] private Transform ContainerIsland;
    [SerializeField] private Transform FollowedGO;
    [SerializeField] private Islands Islands;
    [SerializeField] private float size;

    private List<GameObject> _islandsList = new List<GameObject>();

    public override void Init(GameMaster gameMaster)
    {
        DestroyAllElements(_islandsList);
        DestroyAllElements(_currentSetup.RulesContainers);
        DestroyAllElements(_currentSetup.CellsContainers);
        DestroyAllElements(_ruleCells);
        DestroyAllElements(_gameCells);
        DestroyAllElementsInChildren(_currentSetup.container.gameObject);
        _master = gameMaster;
        gameMaster.OnChanged += OnChanged;
        _currentSetup.Width = _master.GameData.Width;
        _currentSetup.Height = _master.GameData.Height;
        CreateCells();
        PlaceIsland();
        InitSetup();
    }


    private void DestroyAllElementsInChildren(GameObject toDestroy)
    {
        if (toDestroy == null) return;
        foreach (Transform el in toDestroy.GetComponentInChildren<Transform>())
        {
            Destroy(el.gameObject);
        }
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

    private void DestroyAllElements(List<Transform> toDestroy)
    {
        if (toDestroy == null) return;
        foreach (var el in toDestroy)
        {
            Destroy(el.gameObject);
        }

        toDestroy.Clear();
    }

    private void DestroyAllElements(List<GameObject> toDestroy)
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

    private void PlaceIsland()
    {
        Transform newIsland;
    for (int i = 0; i < _currentSetup.Width; i++)
    {
        Transform island;
        
        if (i == 0)
        {
            island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
            newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 90, 0), ContainerIsland);
        }
        else if (i == _currentSetup.Width - 1)
        {
            island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
            newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 180, 0), ContainerIsland);
        }
        else
        {
            island = Islands.Side[Random.Range(0, Islands.Side.Count)];
            newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 180, 0), ContainerIsland);
            
        }
        _islandsList.Add(newIsland.gameObject);
        
    }

    var tempSize = _currentSetup.Width;
    for (int j = 0; j < _currentSetup.Height - 2; j++)
    {
        for (int i = 0; i < _currentSetup.Width; i++)
        {
            Transform island;

            if (i == 0)
            {
                island = Islands.Side[Random.Range(0, Islands.Side.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[tempSize].position, Quaternion.Euler(0, 90, 0),
                    ContainerIsland);
            }
            else if (i == _currentSetup.Width - 1)
            {
                island = Islands.Side[Random.Range(0, Islands.Side.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[tempSize].position, Quaternion.Euler(0, -90, 0),
                    ContainerIsland);
            }
            else
            {
                island = Islands.Center[Random.Range(0, Islands.Center.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[tempSize].position, Quaternion.Euler(0, 180, 0),
                    ContainerIsland);
            }
            _islandsList.Add(newIsland.gameObject);
            tempSize++;
        }
    }
  
    for (int i = _currentSetup.CellsContainers.Count - _currentSetup.Width; i < _currentSetup.CellsContainers.Count; i++)
    {
        Transform island;
        
        if (i == _currentSetup.CellsContainers.Count - _currentSetup.Width)
        {
            island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
            newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 0, 0), ContainerIsland);
        }
        else if (i == _currentSetup.CellsContainers.Count - 1)
        {
            island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
            newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, -90, 0), ContainerIsland);
        }
        else
        {
            island = Islands.Side[Random.Range(0, Islands.Side.Count)];
            newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 0, 0), ContainerIsland);
        }
        _islandsList.Add(newIsland.gameObject);
        
    }
        
        
        
        /*//Transform newIsland;
        int totalSize = _currentSetup.Width * _currentSetup.Height;
        for (int i = 0; i < totalSize; i++)
        {
            int row = i / _currentSetup.Width;
            int column = i % _currentSetup.Width;

            Transform island;
            if (column == 0 && row == 0) // Top-left corner
            {
                island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 90, 0),
                    ContainerIsland);
            }
            else if (column == _currentSetup.Width - 1 && row == 0) // Top-right corner
            {
                island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 180, 0),
                    ContainerIsland);
            }
            else if (column == 0 && row == _currentSetup.Height - 1) // Bottom-left corner
            {
                island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 0, 0),
                    ContainerIsland);
            }
            else if (column == _currentSetup.Width - 1 && row == _currentSetup.Height - 1) // Bottom-right corner
            {
                island = Islands.Angle[Random.Range(0, Islands.Angle.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, -90, 0),
                    ContainerIsland);
            }
            else if (row == 0 || row == _currentSetup.Height - 1) // Top and bottom edges
            {
                island = Islands.Side[Random.Range(0, Islands.Side.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 0, 0),
                    ContainerIsland);
            }
            else if (column == 0 || column == _currentSetup.Width - 1) // Left and right edges
            {
                island = Islands.Side[Random.Range(0, Islands.Side.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 90, 0),
                    ContainerIsland);
            }
            else // Center cells
            {
                island = Islands.Center[Random.Range(0, Islands.Center.Count)];
                newIsland = Instantiate(island, _currentSetup.CellsContainers[i].position, Quaternion.Euler(0, 180, 0),
                    ContainerIsland);
            }

            _islandsList.Add(newIsland.gameObject);
        }*/
    }


    private void CreateCells()
    {
        List<Transform> rulesTop = new List<Transform>();
        List<Transform> rulesRight = new List<Transform>();
        List<Transform> rulesBottom = new List<Transform>();
        List<Transform> rulesLeft = new List<Transform>();

        int width = _currentSetup.Width + 2;
        int height = _currentSetup.Height + 2;
        int tempHeight = height - 1;

        Vector3 pos1 = new Vector3();
        Vector3 pos2 = new Vector3();

        for (int i = 0; i < width; i++)
        {
            var cell = Instantiate(ContainerPrefab.transform, new Vector3((i - 1) * 3, 0, tempHeight * 3),
                Quaternion.Euler(0, 0, 0), _currentSetup.container.transform);
            if (i == 0) pos1 = cell.transform.position;
            if (i > 0 && i < width - 1) rulesTop.Add(cell);
        }

        for (int i = 0; i < height - 2; i++)
        {
            tempHeight -= 1;
            for (int j = 0; j < width; j++)
            {
                var cell = Instantiate(ContainerPrefab.transform, new Vector3((j - 1) * 3, 0, tempHeight * 3),
                    Quaternion.Euler(0, 0, 0), _currentSetup.container.transform);
                if (j == 0) rulesLeft.Add(cell);
                else if (j == width - 1) rulesRight.Add(cell);
                else _currentSetup.CellsContainers.Add(cell);
            }
        }

        tempHeight -= 1;
        for (int i = 0; i < width; i++)
        {
            var cell = Instantiate(ContainerPrefab.transform, new Vector3((i - 1) * 3, 0, tempHeight * 3),
                Quaternion.Euler(0, 0, 0), _currentSetup.container.transform);
            if (i == width - 1) pos2 = cell.transform.position;
            if (i > 0 && i < width - 1) rulesBottom.Add(cell);
        }

        Vector3 position = new Vector3((pos1.x + pos2.x) / 2, 0.1f, (pos1.z + pos2.z) / 2);

        FollowedGO.transform.position = position;

        rulesBottom.Reverse();
        rulesLeft.Reverse();
        _currentSetup.RulesContainers.AddRange(rulesTop);
        _currentSetup.RulesContainers.AddRange(rulesRight);
        _currentSetup.RulesContainers.AddRange(rulesBottom);
        _currentSetup.RulesContainers.AddRange(rulesLeft);
    }
}