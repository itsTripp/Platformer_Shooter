using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EpicTortoiseStudios
{
    public class PopupText : MonoBehaviour
    {
        public LootPickups lootPickup;
        [SerializeField]
        private TextMeshPro _popupText;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            float speed = 2f;
            _popupText.transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
            Destroy(gameObject, .5f);
        }
    }
}