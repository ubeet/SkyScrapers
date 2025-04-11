using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(menuName = "Game/Singletons/RootData", fileName = "RootData", order = 0)]
public class RootData : SerializedScriptableObject
{
    #region Odin
#if UNITY_EDITOR
    private bool ValidateStageIds()
    {
        if (stages.IsEmptyOrInvalid()) return true;
        invalidStagesIdCount = 0;
        invalidStagesIds = new List<string>();
        List<int> uniqueIds = new List<int>();
        foreach (var stage in stages)
        {
            if (uniqueIds.Contains(stage.Id))
            {
                invalidStagesIdCount++;
                invalidStagesIds.Add(stage.name);
            }
            else
            {
                uniqueIds.Add(stage.Id);
            }
        }
        return invalidStagesIdCount < 1;
    }

    private int invalidStagesIdCount = 0;
    private List<string> invalidStagesIds;
    private string InvalidMessage => $"Stages Contains {invalidStagesIdCount} IDs:({string.Join(",", invalidStagesIds)}) same Ids!";
    [OnValueChanged(nameof(ValidateStageIds))]
#endif
    #endregion
    
    [FoldoutGroup("Scene References")] public SceneReference MenuScene;
    [FoldoutGroup("Scene References")] public SceneReference CampaignScene;

#if UNITY_EDITOR
    [OnValueChanged(nameof(ValidateStageIds))]
#endif
    [InfoBox("$InvalidMessage",InfoMessageType.Error,"@invalidStagesIdCount > 0")]
    public List<Stage> stages = new List<Stage>();
    
    public List<Difficult> Difficulties = new List<Difficult>();
    
    
    public Stage CurrentStage;
    public List<HeightContainer> containers;
    
    //size, time, offsetPercent
#if UNITY_EDITOR
    [SuffixLabel("@OdinUtils.FormatTimeMessage(botMinSolveTime)", true)]
#endif 
    [SerializeField] private float botMinSolveTime = 30f;

    [SerializeField] public AnimationCurve BotSolveProgressionCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    public int ServerBoardSize { get; set; }
    
    
    #region Singletone

    public static bool HasInstance => _instance != null;
    private static RootData _instance;

    public static RootData Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR
                _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<RootData>("RootData");
                if (_instance == null)
#endif
                    _instance = UnityEngine.Resources.Load<RootData>("RootData");


                if (_instance == null)
                {
                    Debug.LogError("RootData doesn't found");
                    return null;
                }
            }

            return _instance;
        }
    }

    #endregion

    public void ApplicationExit()
    {
#if UNITY_WEBGL
            exitButton.SetActive(false);
#endif
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }


    public HeightInstance GetByHeight(int height)
    {
        var container = containers.First(c => c.Height == height);
        return container.Get();
    }

    public Stage GetStage(int id)
    {
        if (stages.IsEmptyOrInvalid()) return null;
        foreach (var stage in stages)
            if (stage.Id == id) return stage;
        return null;
    }
}