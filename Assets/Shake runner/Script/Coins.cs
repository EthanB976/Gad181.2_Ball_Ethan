using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class Coins : MonoBehaviour
    {
        [SerializeField] protected private int value;



        private void Start()
        {

        }

        //This makes it so the coins get destoryed when the player comes in contact
        //Also increases how many coins the player has both in the UI and Inventory
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                ShopManager.instance.IncreaseCoins(value);
            }
        }
    }
}

