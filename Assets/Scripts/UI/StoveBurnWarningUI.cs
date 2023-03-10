using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnOnProgressChanged;
        Hide();
    }

    private void StoveCounterOnOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .3f;
        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.isFried();
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
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
