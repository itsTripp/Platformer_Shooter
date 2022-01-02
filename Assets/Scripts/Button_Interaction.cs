using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EpicTortoiseStudios
{

    public class Button_Interaction : MonoBehaviour
    {
        [SerializeField]
        private Button_ScriptableObjects buttonSO;
        [SerializeField]
        private TextMeshProUGUI descriptionText;

        public void OnMouseOver()
        {
            descriptionText.gameObject.SetActive(true);
            descriptionText.text = buttonSO.description;
            if(gameObject.tag == "Yes_Button")
            {
                descriptionText.color = Color.green;
            }
            if(gameObject.tag == "No_Button")
            {
                descriptionText.color = Color.red;
            }
        }

        public void OnMouseExit()
        {
            descriptionText.gameObject.SetActive(false);
        }
    }
}
