using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Stage> defaultStages;
    [SerializeField] private List<Stage> harderStages;
    [SerializeField] private List<Stage> notDefaultStages;
    [SerializeField] private StagePresenter stagePresenterPrefab;
    [SerializeField] private Transform defaultContainer;
    [SerializeField] private Transform harderContainer;
    [SerializeField] private Transform notDefaultContainer;
    [SerializeField] private Button exitButton;
    [SerializeField] private ExitMenu exitMenu;
    void Start()
    {
        exitButton.onClick.AddListener(exitMenu.OpenMenuAbove);
        foreach (var el in defaultStages)
        {
            StagePresenter presenter = Instantiate(stagePresenterPrefab, defaultContainer);
            presenter.Init(el);
        }
        foreach (var el in harderStages)
        {
            StagePresenter presenter = Instantiate(stagePresenterPrefab, harderContainer);
            presenter.Init(el);
        }
        foreach (var el in notDefaultStages)
        {
            StagePresenter presenter = Instantiate(stagePresenterPrefab, notDefaultContainer);
            presenter.Init(el);
        }
    }
}
