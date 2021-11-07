using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Item", menuName = "Loot_Items")]
public class LootPickups : ScriptableObject
{
    public Sprite lootSprite;
    public AudioClip audioClip;
    public LootType lootType;
    public enum LootType
    {
        None,
        Experience,
        Pistol,
        Shotgun
    };
}
