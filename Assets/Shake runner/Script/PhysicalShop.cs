using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
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
            //this checks if the player is inside the shop triggerzone
            if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q))
            {
                //these are to enabkle the shop popup along with the cursor
                Debug.Log("Opened Shop");

                shopCanvas.enabled = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            //this disables the cursor and the shop once the player has left
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            shopCanvas.enabled = false;
        }
    }
}


