using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    [SerializeField]
    private int max;
    [SerializeField]
    private int current;

    public int AddItem(int addAmount)
    {
        current = Mathf.Clamp(current + addAmount, 0, max);
        return current;
    }

    public int GetItemCnt()
    {
        return current;
    }
}