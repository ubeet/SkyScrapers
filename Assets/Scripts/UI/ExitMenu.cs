using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExitMenu : MenuAbove
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        exitButton.onClick.AddListener(Exit);
        closeButton.onClick.AddListener(CloseMenuAbove);
    }
}