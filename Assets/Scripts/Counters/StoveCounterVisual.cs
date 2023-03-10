using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StoveCounterVisual : MonoBehaviour
{
    [FormerlySerializedAs("baseCounter")] [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounterOnOnStateChanged;
    }

    private void StoveCounterOnOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
