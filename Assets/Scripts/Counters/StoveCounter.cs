using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                    {
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.ouput, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    
                    if (burningTimer >= burningRecipeSO.burningTimerMax)
                    {
                        burningTimer = 0f;
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.ouput, this);

                        state = State.Burned;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                        {
                            state = state
                        });
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {   // give obj to stove if has recipe
            if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                state = State.Frying;
                fryingTimer = 0f;
                
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                {
                    state = state
                });
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                });
            }
        }else if (!player.HasKitchenObject() && HasKitchenObject())
        {   // give obj to player
            GetKitchenObject().SetKitchenObjectParent(player);
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
            {
                state = state
            });
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = 0f
            });
        }else if (!player.HasKitchenObject() && !GetKitchenObject())
        {
            
        }
        else
        {   // both counter and player has obj
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {   // player is carrying a plate
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                {
                    GetKitchenObject().DestroySelf();
                    
                    state = State.Idle;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                    {
                        state = state
                    });
            
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                    {
                        progressNormalized = 0f
                    });
                }
            }
        }
    }

    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenRecipeSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenRecipeSO);
        return fryingRecipeSO != null;
    }
    
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO cuttingRecipeSo = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSo)
        {
            return cuttingRecipeSo.ouput;
        }
        else
        {
            return null;
        }
    }

    public FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenRecipeSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenRecipeSO)
                return fryingRecipeSO;
        }

        return null;
    }
    public BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenRecipeSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenRecipeSO)
                return burningRecipeSO;
        }

        return null;
    }


    public bool isFried()
    {
        return state == State.Fried;
    }
}
