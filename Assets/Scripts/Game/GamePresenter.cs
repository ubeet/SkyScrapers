using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GamePresenter : MonoBehaviour
{
    protected GameMaster _master;
    public List<RulePresenter> _ruleCells = new List<RulePresenter>();
    public List<CellPresenter> _gameCells = new List<CellPresenter>();

    [SerializeField] private Decorations decorations;

    //protected List<GamePresenterSetup> _setups;
    [SerializeField] protected GamePresenterSetup _currentSetup;

    public virtual void Init(GameMaster gameMaster)
    {
    }

    public void InitSetup(float gridSize = 0)
    {
        if (_currentSetup != null)
        {
            RulePresenter3D sd;

            int totalRules = (_currentSetup.Width + _currentSetup.Height) * 2;
            for (int i = 0; i < totalRules; i++)
            {
                var rc = Instantiate(_currentSetup.RulePresenterPrefab, _currentSetup.RulesContainers[i]);
                rc.Init(i);
                if (rc.GetComponent<RulePresenter3D>() != null)
                {
                    rc.transform.eulerAngles = new Vector3(rc.transform.eulerAngles.x,
                        ((int)(i / _currentSetup.Width) + 2) * 90f, rc.transform.eulerAngles.z);
                    rc.text.transform.eulerAngles = new Vector3(rc.image.eulerAngles.x, rc.text.transform.eulerAngles.y,
                        ((int)(i / _currentSetup.Width) + 2) * 90f);
                }
                else if (rc.GetComponent<RulePresenter2D>() != null)
                {
                    rc.transform.eulerAngles = new Vector3(rc.transform.eulerAngles.x, rc.transform.eulerAngles.y,
                        ((int)(i / _currentSetup.Width) + 2) * -90f);
                    rc.text.transform.localEulerAngles = new Vector3(rc.image.eulerAngles.x,
                        rc.text.transform.eulerAngles.y, ((int)(i / _currentSetup.Width) + 2) * 90f);
                }

                _ruleCells.Add(rc);
            }

            int totalCells = _currentSetup.Width * _currentSetup.Height;
            for (int i = 0; i < totalCells; i++)
            {
                List<int> rotations = new List<int>();
                var gc = Instantiate(_currentSetup.CellPresenterPrefab, _currentSetup.CellsContainers[i]);
                gc.Init(i, OnChange, Mathf.Max(_currentSetup.Width, _currentSetup.Height), _master, gridSize);
                var gc3d = gc.GetComponent<CellPresenter3D>();
                if (gc3d != null)
                {
                    var places = gc3d.buildings;
                    if (i - _currentSetup.Width >= 0) rotations.Add(90);
                    if (i + _currentSetup.Width < totalCells) rotations.Add(-90);
                    if ((i + 1) / _currentSetup.Width == i / _currentSetup.Width) rotations.Add(180);
                    if ((i - 1) / _currentSetup.Width == i / _currentSetup.Width && i - 1 >= 0) rotations.Add(0);
                    
                    gc.transform.rotation = Quaternion.Euler(0, rotations[Random.Range(0, rotations.Count)], 0);
                }

                _gameCells.Add(gc);
            }
        }
    }

    private void OnChange(int index, int value)
    {
        _master.OnNewSet?.Invoke(this, index, value);
    }

    protected void OnChanged(GamePresenter sender, bool force)
    {
        Redraw(force);
    }

    protected virtual void Redraw(bool force)
    {
        var data = _master.GameData;
        for (int i = 0; i < data.RuleCells.Count; i++)
        {
            int dataRC = data.RuleCells[i];
            _ruleCells[i].Set(dataRC, force);
        }

        for (int i = 0; i < data.GameCells.Count; i++)
        {
            int dataGC = 0;
            if (data.GameCells[i].Count == 1)
                dataGC = data.GameCells[i][0];
            
            if (dataGC > 0 && !_master._isPresentersInited)
            {
                _gameCells[i]._isChangeable = false;
            }
            _gameCells[i].Set(data.GameCells[i], force);
        }
    }

    private void Reset()
    {
        foreach (var cell in _ruleCells)
            cell.Reset();
        foreach (var cell in _gameCells)
            cell.Reset();
    }
}