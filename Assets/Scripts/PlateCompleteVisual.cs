using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_Gameobject
    {
        public KitchenObjectSO KitchenObjectSo;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_Gameobject> kitchenObjectSOGameobjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;
        
        foreach (KitchenObjectSO_Gameobject kitchenObjectSOGameobject in kitchenObjectSOGameobjectList)
        {
            kitchenObjectSOGameobject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_Gameobject kitchenObjectSOGameobject in kitchenObjectSOGameobjectList)
        {
            if (kitchenObjectSOGameobject.KitchenObjectSo == e.KitchenObjectSO)
            {
                kitchenObjectSOGameobject.gameObject.SetActive(true);
            }
        }
    }
}
