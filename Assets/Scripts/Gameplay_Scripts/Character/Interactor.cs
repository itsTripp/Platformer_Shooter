using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EpicTortoiseStudios
{
    public class Interactor : MonoBehaviour
    {
        public Weapon equipableWeapon = null;

        public void SelectWeapon(Weapon weapon)
        {
            
            UnSelectWeapon();

            equipableWeapon = weapon;
        }

        public void UnSelectWeapon()
        {
            if (equipableWeapon != null)
            {
                equipableWeapon.UnSelectable();
                equipableWeapon = null;
            }
        }
    }
}

