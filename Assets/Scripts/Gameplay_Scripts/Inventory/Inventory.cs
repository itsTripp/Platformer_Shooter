using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public enum AmmoType
    {
        SmallCaliber = 0,
        MediumCaliber = 1,
        Shell = 2,
        Energy = 3
    }
    public enum EquipmentType
    {
        Grenade = 0,
        Healing = 1
    }

    [SerializeField]
    private List<InventoryAmmo> Ammos = new List<InventoryAmmo>();
    [SerializeField]
    private List<InventoryEquipment> Equipment = new List<InventoryEquipment>();

    public int AddAmmo(Inventory.AmmoType ammoType, int amount)
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

    public int GetAmmo(Inventory.AmmoType ammoType)
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

    public int AddEquipment(Inventory.EquipmentType equipType, int amount)
    {
        int iNewEquipCnt = 0;

        foreach (InventoryEquipment equip in Equipment)
        {
            if (equip.type == equipType)
            {
                iNewEquipCnt = equip.AddItem(amount);
                break;
            }
        }

        return iNewEquipCnt;
    }

    public int GetEquipment(Inventory.EquipmentType equipType)
    {
        int iCurrentEquipCnt = 0;

        foreach (InventoryEquipment equip in Equipment)
        {
            if (equip.type == equipType)
            {
                iCurrentEquipCnt = equip.GetItemCnt();
                break;
            }
        }

        return iCurrentEquipCnt;
    }
}