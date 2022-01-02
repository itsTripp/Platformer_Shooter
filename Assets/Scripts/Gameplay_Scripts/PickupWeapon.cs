using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EpicTortoiseStudios
{
    public class PickupWeapon : MonoBehaviour
    {
        public Weapon weapon;
        private Player _player;
        private UIManager _uiManager;
        public GameObject PopupText;

        private void Start()
        {
            _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Right_Weapon")
            {
                if (GameControl.gameControl.currentRightWeapon != null)
                {
                    if (GameControl.gameControl.currentRightWeapon == weapon)
                    {
                        GameControl.gameControl.currentRightWeapon = weapon;
                        other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                        GameControl.gameControl.rightWeaponCurrentAmmo = GameControl.gameControl.rightWeaponCurrentAmmo + weapon.ammoPickupAmount;
                        _uiManager.UpdateWeapon();
                        _uiManager.UpdateAmmoCount();
                        var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                        go.GetComponent<TextMeshPro>().text = weapon.ammoCountText;
                        Destroy(gameObject);
                    }
                    else
                    {
                        GameControl.gameControl.currentRightWeapon = weapon;
                        other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                        GameControl.gameControl.rightWeaponCurrentAmmo = GameControl.gameControl.rightWeaponCurrentAmmo + weapon.ammoPickupAmount;
                        _uiManager.UpdateWeapon();
                        _uiManager.UpdateAmmoCount();
                        var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                        go.GetComponent<TextMeshPro>().text = weapon.weaponText;
                        Destroy(gameObject);
                    }

                }
                else
                {
                    GameControl.gameControl.currentRightWeapon = weapon;
                    other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                    GameControl.gameControl.rightWeaponCurrentAmmo = GameControl.gameControl.rightWeaponCurrentAmmo + weapon.ammoPickupAmount;
                    _uiManager.UpdateWeapon();
                    _uiManager.UpdateAmmoCount();
                    var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                    go.GetComponent<TextMeshPro>().text = weapon.weaponText;
                    Destroy(gameObject);
                }

            }
            if (other.tag == "Left_Weapon")
            {
                if (GameControl.gameControl.currentLeftWeapon != null)
                {
                    if (GameControl.gameControl.currentLeftWeapon == weapon)
                    {
                        GameControl.gameControl.currentLeftWeapon = weapon;
                        other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                        GameControl.gameControl.leftWeaponCurrentAmmo = GameControl.gameControl.leftWeaponCurrentAmmo + weapon.ammoPickupAmount;
                        _uiManager.UpdateWeapon();
                        _uiManager.UpdateAmmoCount();
                        var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                        go.GetComponent<TextMeshPro>().text = weapon.ammoCountText;
                        Destroy(gameObject);
                    }
                    else
                    {
                        GameControl.gameControl.currentLeftWeapon = weapon;
                        other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                        GameControl.gameControl.leftWeaponCurrentAmmo = GameControl.gameControl.leftWeaponCurrentAmmo + weapon.ammoPickupAmount;
                        _uiManager.UpdateWeapon();
                        _uiManager.UpdateAmmoCount();
                        var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                        go.GetComponent<TextMeshPro>().text = weapon.weaponText;
                        Destroy(gameObject);
                    }

                }
                else
                {
                    GameControl.gameControl.currentLeftWeapon = weapon;
                    other.transform.GetComponent<SpriteRenderer>().sprite = weapon.currentWeaponSprite;
                    GameControl.gameControl.leftWeaponCurrentAmmo = GameControl.gameControl.leftWeaponCurrentAmmo + weapon.ammoPickupAmount;
                    _uiManager.UpdateWeapon();
                    _uiManager.UpdateAmmoCount();
                    var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                    go.GetComponent<TextMeshPro>().text = weapon.weaponText;
                    Destroy(gameObject);
                }
            }
        }
        
    }
}
