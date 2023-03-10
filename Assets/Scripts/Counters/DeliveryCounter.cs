using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject() is PlateKitchenObject)
            {
                DeliveryManager.Instance.DeliverRecipe(player.GetKitchenObject() as PlateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
    
}
