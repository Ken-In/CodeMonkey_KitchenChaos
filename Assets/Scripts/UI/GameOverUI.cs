using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDeliveredText;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManagerOnStateChanged;
        Hide();
    }

    private void KitchenGameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipeDeliveredText.text = DeliveryManager.Instance.GetDeliveredRecipeCount().ToString();

        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        mainMenuButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
