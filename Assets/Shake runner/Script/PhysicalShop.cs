using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalShop : MonoBehaviour
{
    public ShopManager shopMenu; 

    [SerializeField] public Canvas shopCanvas ;

    private bool shopActive = false;

    private void Start()
    {
        shopCanvas.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == true && Input.GetKeyDown(KeyCode.Q))
        {
            //shopMenu.gameObject.SetActive(!shopMenu.gameObject.activeSelf);
            Debug.Log("Bruh");

            shopCanvas.enabled = true;
        }


    }
}
