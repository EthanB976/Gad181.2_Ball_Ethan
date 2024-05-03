using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EB
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;

        [SerializeField] public int currentCoins;

        [SerializeField] public int itemAmount = 20;
        public static ShopManager instance;

        [SerializeField] private Button Sliding;
        [SerializeField] private Button WallRunning;
        [SerializeField] private Button Dashing;
        [SerializeField] private Button Climbing;

        void Awake()
        {
            instance = this;

            Dashing.enabled = false;
            currentCoins = 0;
        }

        private void Start()
        {
            //Update coins and disables buttons from the start 
            coinText.text = "You have " + currentCoins.ToString() + " Coins";
        }

        //This increases the coin amount every time you pick up a coin
        public void IncreaseCoins(int v)
        {
            currentCoins += v;
            UpdateUI();
        }
        //Updates the UI once the player has gained or spent coins
        public void UpdateUI()
        {
            coinText.text = "You have " + currentCoins.ToString() + " Coins";
        }
        //This works with the UI update to subtract coins whenever an item is purchased
        public void SubtractCoins()
        {
            currentCoins -= 20;
        }

        private void Update()
        {
            //This is for enabling the button too be able to be clicked once the player has a certain number of coins
            if (currentCoins >= 20)
            {
                Dashing.enabled = true;
            }
            else
            {
                Dashing.enabled = false;
            }

            if (currentCoins >= 20)
            {
                Sliding.enabled = true;
            }
            else
            {
                Sliding.enabled = false;
            }

            if (currentCoins >= 20)
            {
                Climbing.enabled = true;
            }
            else
            {
                Climbing.enabled = false;
            }
            if (currentCoins >= 20)
            {
                WallRunning.enabled = true;
            }
            else
            {
                WallRunning.enabled = false;
            }
        }

    }
}

