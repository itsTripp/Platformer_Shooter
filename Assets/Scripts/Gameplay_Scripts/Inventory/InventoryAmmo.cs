using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [System.Serializable]
    public class InventoryAmmo : InventoryItem
    {
        [SerializeField]
        public CommonEnums.AmmoType type;
    }
}

