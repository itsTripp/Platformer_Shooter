using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
    public class Weapon : ScriptableObject
    {
        public Sprite currentWeaponSprite;
        public GameObject bulletPrefab;
        public WeaponType weaponType;
        public float fireRate = 1;
        public int damage = 1;
        public int maximumAmmo;
        public enum WeaponType
        {
            None,
            Pistol,
            Shotgun
        };

        private void Awake()
        {

        }

        public void ShootRightWeapon()
        {
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, GameObject.Find("Right_FirePoint")
                    .transform.position, Quaternion.Euler(0, 0, -90));
            }
        }

        public void ShootLeftWeapon()
        {
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, GameObject.Find("Left_FirePoint")
                    .transform.position, Quaternion.Euler(0, 0, 90));
            }
        }
    }
}
