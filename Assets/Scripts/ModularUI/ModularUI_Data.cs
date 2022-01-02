using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicTortoiseStudios
{
    [CreateAssetMenu(menuName = "Modular UI Data")]
    public class ModularUI_Data : ScriptableObject
    {
        [Header("Modular Button")]
        public Sprite buttonSprite;
        public SpriteState buttonSpriteState;

        public Color defaultColor;
        public Sprite defaultIcon;

        public Color confirmColor;
        public Sprite confirmIcon;

        public Color declineColor;
        public Sprite declineIcon;

        public Color warningColor;
        public Sprite warningIcon;

        [Header("Text")]
        public string text;
    }
}