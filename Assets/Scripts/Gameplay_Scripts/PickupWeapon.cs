using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class PickupWeapon : MonoBehaviour
    {
        public Weapon weapon;
        private Player _player;
        private UIManager _uiManager;

        private void Start()
        {
            _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Right_Weapon")
            {
                GameControl.gameControl.currentRightWeapon = weapon;
                other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                GameControl.gameControl.rightWeaponCurrentAmmo = weapon.maximumAmmo;
                _uiManager.UpdateWeapon();
                Destroy(gameObject);
            }
            if (other.tag == "Left_Weapon")
            {
                GameControl.gameControl.currentLeftWeapon = weapon;
                other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                GameControl.gameControl.leftWeaponCurrentAmmo = weapon.maximumAmmo;
                _uiManager.UpdateWeapon();
                Destroy(gameObject);
            }
        }
    }
}
