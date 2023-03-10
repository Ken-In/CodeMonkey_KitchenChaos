using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        
        optionButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show(Show);
            Hide();
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        Hide();
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManagerOnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManagerOnGameUnpaused;
    }

    private void KitchenGameManagerOnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManagerOnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
