using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {   // give obj to counter
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else if (!player.HasKitchenObject() && HasKitchenObject())
        {   // give obj to player
            GetKitchenObject().SetKitchenObjectParent(player);
        }        
        else if(!player.HasKitchenObject() && !HasKitchenObject())
        {   // both counter and player has not obj
        }
        else
        {   // both counter and player has obj
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject playerPlate))
            {   // player is carrying a plate
                if (playerPlate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                {
                    GetKitchenObject().DestroySelf();
                }
            }else if (GetKitchenObject().TryGetPlate(out PlateKitchenObject counterPlate))
            {   // counter is carrying a plate
                if (counterPlate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}
