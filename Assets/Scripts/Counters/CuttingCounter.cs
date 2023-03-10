using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public new static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {   // give obj to counter
            if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
                
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
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
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {   // player is carrying a plate
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                {
                    GetKitchenObject().DestroySelf();
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {   // cut the obj
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
                
            // loop every time when counting , need optimazation
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                
                GetKitchenObject().DestroySelf();
                
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSo)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSo)
        {
            return cuttingRecipeSo.ouput;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput (KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSo in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == cuttingRecipeSo.input)
                return cuttingRecipeSo;
        }
        return null;
    }
}
