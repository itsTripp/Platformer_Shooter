using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentChance : MonoBehaviour
{
    [SerializeField][Range(0, 100)] int percent;

    public bool GetSuccess()
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance <= percent)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
