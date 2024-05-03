using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class PurchasingItem : MonoBehaviour
    {
        [SerializeField] private ShopManager purchase;


        public void PurchaseItem()
        {
            Debug.Log("Purchased item");

            // Also like to deduct coins
            purchase.SubtractCoins();
            purchase.UpdateUI();

            //To hide the button and make it so it cant be purchased
            //We can Deactivate the gameobject

            gameObject.SetActive(false);
        }
    }
}

