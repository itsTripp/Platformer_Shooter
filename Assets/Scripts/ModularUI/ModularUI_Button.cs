using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicTortoiseStudios
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class ModularUI_Button : ModularUI
    {
        public enum ButtonType
        {
            Default,
            Confirm,
            Decline,
            Warning
        }

        protected Image image;
        [SerializeField]
        protected Image icon;
        protected Button button;
        public Text text;

        public ButtonType buttonType;

        public override void Awake()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
            base.Awake();
        }
        protected override void OnSkinUI()
        {
            base.OnSkinUI();



            if (text != null)
            {
                text.text = skinData.text.ToString();
            }

            button.transition = Selectable.Transition.SpriteSwap;
            button.targetGraphic = image;

            image.sprite = skinData.buttonSprite;
            image.type = Image.Type.Sliced;
            button.spriteState = skinData.buttonSpriteState;

            switch (buttonType)
            {
                case ButtonType.Confirm:
                    image.color = skinData.confirmColor;
                    icon.sprite = skinData.confirmIcon;
                    break;

                case ButtonType.Decline:
                    image.color = skinData.declineColor;
                    icon.sprite = skinData.declineIcon;
                    break;

                case ButtonType.Warning:
                    image.color = skinData.warningColor;
                    icon.sprite = skinData.warningIcon;
                    break;

                case ButtonType.Default:
                    image.color = skinData.defaultColor;
                    icon.sprite = skinData.defaultIcon;
                    break;
            }
        }
    }
}
