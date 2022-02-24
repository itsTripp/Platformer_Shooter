using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class Inventory : MonoBehaviour
    {

        [SerializeField]
        private List<InventoryAmmo> Ammos = new List<InventoryAmmo>();
        [SerializeField]
        private List<InventoryEquipment> Equipment = new List<InventoryEquipment>();

        public int AddAmmo(CommonEnums.AmmoType ammoType, int amount)
        {
            int iNewAmmoCnt = 0;

            foreach (InventoryAmmo ammo in Ammos)
            {
                if (ammo.type == ammoType)
                {
                    iNewAmmoCnt = ammo.AddItem(amount);
                    break;
                }
            }

            return iNewAmmoCnt;
        }

        public int GetAmmo(CommonEnums.AmmoType ammoType)
        {
            int iCurrentAmmoCnt = 0;

            foreach (InventoryAmmo ammo in Ammos)
            {
                if (ammo.type == ammoType)
                {
                    iCurrentAmmoCnt = ammo.GetItemCnt();
                    break;
                }
            }

            return iCurrentAmmoCnt;
        }
    }
}
