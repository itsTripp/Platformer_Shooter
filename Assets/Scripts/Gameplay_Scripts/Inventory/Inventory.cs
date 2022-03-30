using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public bool InfiniteAmmo = false;

        [SerializeField]
        private List<InventoryAmmo> Ammos = new List<InventoryAmmo>();
        [SerializeField]
        private List<InventoryEquipment> Equipment = new List<InventoryEquipment>();

        public int AddAmmo(CommonEnums.AmmoType ammoType, int amount)
        {
            int iNewAmmoCnt = 0;

            if (InfiniteAmmo)
            {
                //If infinite ammo negative ammo cost will be zeroed, but picking up ammo (A + amount), will be allowed.
                amount = Mathf.Max(amount, 0);
            }

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

            if (InfiniteAmmo)
            {
                iCurrentAmmoCnt = 999;
            } else
            {
                foreach (InventoryAmmo ammo in Ammos)
                {
                    if (ammo.type == ammoType)
                    {
                        iCurrentAmmoCnt = ammo.GetItemCnt();
                        break;
                    }
                }
            }
            

            

            return iCurrentAmmoCnt;
        }
    }
}
