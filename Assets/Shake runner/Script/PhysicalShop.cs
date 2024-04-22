using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalShop : MonoBehaviour
{
    public ShopManager shopMenu;

    [SerializeField] public Canvas shopCanvas;


    private void Start()
    {
        shopCanvas.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q))
        {
           
            Debug.Log("Bruh");

            shopCanvas.enabled = true;
        }

    }
}

