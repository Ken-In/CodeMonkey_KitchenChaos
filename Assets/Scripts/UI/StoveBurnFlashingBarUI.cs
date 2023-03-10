using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string ISFLASHING = "IsFlashing";
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnOnProgressChanged;
        animator.SetBool(ISFLASHING, false);
    }

    private void StoveCounterOnOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .3f;
        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.isFried();
        
        animator.SetBool(ISFLASHING, show);
    }
}
