using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public WeaponItem rightweapon;
        public WeaponItem leftweapon;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }


        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightweapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftweapon, true);
        }




    }
}

