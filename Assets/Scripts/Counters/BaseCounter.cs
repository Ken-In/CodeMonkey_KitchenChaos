using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;
    
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    
    [SerializeField] protected Transform counterTopPoint;
    private KitchenObject kitchenObject;
    
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact(), you need to implement this method!");
    }
    
    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("BaseCounter.InteractAlternate(), you need to implement this method!");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
