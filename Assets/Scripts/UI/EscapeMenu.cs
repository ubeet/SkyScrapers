using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EscapeMenu : MenuAbove
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        menuButton.onClick.AddListener(OpenMainMenu);
        closeButton.onClick.AddListener(CloseMenuAbove);
    }
    
    public void OpenMainMenu()
    {
        FindObjectOfType<SceneFader>().FadeToScene(0);
    }
}
