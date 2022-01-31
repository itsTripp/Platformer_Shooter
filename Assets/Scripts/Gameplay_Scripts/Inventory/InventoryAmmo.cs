using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryAmmo : InventoryItem
{
    [SerializeField]
    public Inventory.AmmoType type;
}