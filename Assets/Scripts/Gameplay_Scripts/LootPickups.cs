using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [CreateAssetMenu(fileName = "New Loot Item", menuName = "Loot_Items")]
    public class LootPickups : ScriptableObject
    {
        public Sprite lootSprite;
        public AudioClip audioClip;
        public string popupText;
        public LootType lootType;
        public enum LootType
        {
            None,
            Experience,
            Pistol,
            Shotgun
        };
        public int ammoPickupAmount;
    }
}
