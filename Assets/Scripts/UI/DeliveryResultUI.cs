using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "PopUp";
    
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeleveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += InstanceOnOnRecipeFailed;
        
        gameObject.SetActive(false);
    }

    private void InstanceOnOnRecipeFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        messageText.text = "DELIVERY\nFAILED";
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        animator.SetTrigger(POPUP);
    }

    private void DeleveryManagerOnRecipeSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        messageText.text = "DELIVERY\nSUCCESS";
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        animator.SetTrigger(POPUP);
    }
}
