using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EpicTortoiseStudios
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField]
        private int _pickupID;
        [SerializeField]
        private AudioClip _audioClip;
        private Player _player;
        public LootPickups lootPickup;
        [SerializeField]
        private GameObject popupText;
        private UIManager _uiManager;



        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player == null)
            {
                Debug.LogError("Player is Null on PickupItem");
            }
            _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
            if(_uiManager == null)
            {
                Debug.LogError("UI Manager is Null on PickupItem");
            }
        }


        // Update is called once per frame
        void Update()
        {
            //Screen Wrapping
            if (transform.position.x > 10.3f)
            {
                transform.position = new Vector3(-10.3f, transform.position.y, 0);
            }
            if (transform.position.x < -10.3f)
            {
                transform.position = new Vector3(10.3f, transform.position.y, 0);
            }
            if (transform.position.y < -1.5f)
            {
                transform.position = new Vector3(transform.position.x, 11f, 0);
            }
            if (transform.position.y > 11f)
            {
                transform.position = new Vector3(transform.position.x, -1.5f, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                if(lootPickup.lootType == LootPickups.LootType.Experience)
                {
                    _uiManager.AddExperience(_player._experienceOnPickup);
                    Destroy(gameObject);
                    Debug.Log("XP Pickup");
                }
                if(lootPickup.lootType == LootPickups.LootType.Pistol)
                {
                    GameControl.gameControl.pistolAmmo =+ lootPickup.ammoPickupAmount;
                    _uiManager.UpdateAmmoCount();
                    Destroy(gameObject);
                    Debug.Log("Pistol Ammo");
                }
                if(lootPickup.lootType == LootPickups.LootType.Shotgun)
                {
                    GameControl.gameControl.shotgunAmmo =+ lootPickup.ammoPickupAmount;
                    _uiManager.UpdateAmmoCount();
                    Destroy(gameObject);
                    Debug.Log("Shotgun Ammo");
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                var go = Instantiate(popupText, transform.position, Quaternion.identity);
                go.GetComponent<TextMeshPro>().text = lootPickup.popupText;
            }
        }
    }
}
