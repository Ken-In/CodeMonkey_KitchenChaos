using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopUp";
    
    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManagerOnStateChanged;
        gameObject.SetActive(false);
    }

    private void KitchenGameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumer = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
        countdownText.text = countdownNumer.ToString();

        if (previousCountdownNumber != countdownNumer)
        {
            previousCountdownNumber = countdownNumer;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound(transform.position, 0.2f);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    
}
