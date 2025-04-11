using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private List<GamePresenter> Presenters = new List<GamePresenter>();
    [SerializeField] private ResultResolverBase resultResolver;
    [SerializeField] private TimerPresenter timerPresenter;
    [SerializeField] private EscapeMenu escapeMenu;
    
    float _timeHasPassed;


    public Action<GamePresenter, bool> OnChanged;
    public Action<GamePresenter, int, int> OnNewSet;
    
    public int seed;
    public float timeLeft;

    public bool gameStarted = false;

    public GameData GameData { get; private set; }
    public Stage ThisGameStage { get; private set; }
    
    private bool _isGameEnd;
    public bool _isPresentersInited;

    public void Start()
    {
        StartNewGame();
    }


    public void StartNewGameAutoMagically()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        
        OnChanged = null;
        gameStarted = true;
        OnNewSet = null;
        timeLeft = RootData.Instance.CurrentStage.timeToComplite;
        OnNewSet += onNewSet;
        seed = Random.Range(0, Int32.MaxValue);
        Init();
    }
    
    private void Update()
    {
        if (!gameStarted) return;
        if (_isGameEnd) return;
        
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
            escapeMenu.OpenMenuAbove();
        if(!timerPresenter.gameObject.activeSelf)
            timerPresenter.gameObject.SetActive(true);
        timerPresenter.TimeCounter(ref timeLeft);

        _timeHasPassed += Time.deltaTime;
    }

    private void onNewSet(GamePresenter sender, int index, int value)
    {
        
        if (value == 0)
        {
            GameData.GameCells[index].Clear();
        }
        else
        {
            if (!Keyboard.current.shiftKey.isPressed)
            {
                GameData.GameCells[index].Clear();
                GameData.GameCells[index].Add(value);
            }
            else
            {
                if (GameData.GameCells[index].Contains(value))
                    GameData.GameCells[index].Remove(value);
                else
                    GameData.GameCells[index].Add(value);
            }
        }
        
        OnChanged?.Invoke(sender, false);
        
        
        ChangeRulesColor();
        ChangeCellsColor();
        
        if (GameData.CheckWin())
        {
            _isGameEnd = true;
            
            if (resultResolver == null) return;
            resultResolver.Win();
        }
    }


    private void ChangeRulesColor()
    {
        List<int> ruleIndexes = GameData.GetIncorrectRules();
        

        foreach (var presenter in Presenters)
        {
            for (int i = 0; i < presenter._ruleCells.Count; i++) 
                presenter._ruleCells[i].text.color = ruleIndexes.Contains(i) ? Color.red : Color.white;
        }
        
    }
    
    private void ChangeCellsColor()
    {
        List<int> cellIndexes = GameData.GetIncorrectCells();

        foreach (var presenter in Presenters)
        {
            if (presenter.GetComponent<GamePresenter2D>() != null)
                for (int i = 0; i < presenter._gameCells.Count; i++) 
                    presenter._gameCells[i].text.color = cellIndexes.Contains(i) ? Color.red : Color.white;
        }
        
    }

    public void Init()
    {
        _timeHasPassed = 0;
        timerPresenter.SetZeroTime();
        Random.InitState(seed);
        ThisGameStage = RootData.Instance.CurrentStage;
        GameData = ThisGameStage.GetData(seed);
        HeightPickerBehaviour.Instance.Init(GameData.MaxSize);
        _isPresentersInited = false;
        foreach (var presenter in Presenters)
            presenter.Init(this);
        OnChanged?.Invoke(null, true);
        _isPresentersInited = true;
    }
    
}