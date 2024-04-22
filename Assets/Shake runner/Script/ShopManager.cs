using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    [SerializeField] public int currentCoins = 0;

    public static ShopManager instance;

    void Awake()
    {
        instance = this;
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
    public void SubtractCoins(int amount)
    {
        currentCoins -= amount;
    }

}