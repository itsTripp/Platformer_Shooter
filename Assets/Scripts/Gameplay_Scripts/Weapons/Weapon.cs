using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EpicTortoiseStudios
{
    public class Weapon : MonoBehaviour
    {
        // Every weapon will have a Weapon script
        // This is the main script in which majority of the functionality is added.

        // The player should only call to fire the weapon, the weapon should handle ammo, fire rate, etc.

        // Public Functions include the following:
        // FireWeapon
        // AddAmmo

        // Private Functions include the following:
        // RemoveAmmo
        // CanFire
        // SpawnProjectile
        // CalculateSpread

        // Things to keep in mind
        // 1. In future modules will be added, a list of modules will be cycled through for different actions
        // 1a. Example would be Projectile Spawning Modules that would all be called when a projectile is spawned.

        // Global Serialized

        [SerializeField]
        private string weaponName; // Name that will be used for HUD elements
        [SerializeField]
        private string weaponDescription; // Description that will be used for HUD elements
        [SerializeField]
        private CommonEnums.AmmoType ammoType; // Ammo Type that is used, checks the inventory of the player and uses that ammo.
        [SerializeField]
        private int providedAmmo = 5; // # of Ammo given when the weapon is picked up.
        [SerializeField]
        private float fireRate = .5f; // Rate at which the fire command can trigger
        [SerializeField]
        private float damageMultiplyer = 1; // Damage is set on the projectile, this multiplyer alters the damage. (I.E Projectile Damage = 10 - Weapon Multiplyer = 1.2 : Projectile New Damage = 12)
        [SerializeField]
        private GameObject projectilePrefab; // Projectile object to instantiate
        [SerializeField]
        private float projectileSpeed = 750; // Speed to be multiplied against the spawn velocity to determine speed of projectile.
        [SerializeField]
        private bool eachProjectileCostAmmo = false; // Does each spawned projectile cost ammo, or just the shot?
        [SerializeField]
        private int projectileAmmoCost = 1; // Number of Ammo required per projectile/per shot
        [SerializeField]
        private int projectilesPerShot = 1; // Number of projectiles spawned per fire command
        [SerializeField]
        private int projectilesPerDelay = 1; // Number of projectiles spawned at the same time each delay.
        [SerializeField]
        private float projectileSpawnDelay = 0; // Time between each of the per shot projectiles - 0 fires all projectiles at once.
        [SerializeField]
        private float projectileSpreadFactor = 0; // Amount of variance for projectile spread.
        [SerializeField]
        private float recoilIntensity = 10f;
        [SerializeField]
        private float throwSpin = 50f;
        [SerializeField]
        private float throwSpeed = 100f;

        // Hidden Global Variables that don't change much
        private Inventory inventory;
        private Player player;
        private Transform firePoint;
        [SerializeField]
        private Rigidbody2D rigidbody;
        [SerializeField]
        private BoxCollider2D collider;

        [SerializeField] UnityEvent m_Equip;
        [SerializeField] UnityEvent m_Unequip;
        [SerializeField] UnityEvent m_ShotFire;
        [SerializeField] UnityEvent m_ProjectileFire;
        [SerializeField] UnityEvent m_NoAmmo;

        // Hidden Global Variables that are constantly changing
        private bool bCanShoot;
        private bool bCanSpawnNextProjectile;
        private float equipFactor = 1;

        private void Start()
        {
            firePoint = this.transform.Find("FirePoint");
        }

        public bool EquipToPlayer(Transform equipPosition, Inventory inv, Player character)
        {
            bool bEquipped = false;

            rigidbody.simulated = false;
            collider.enabled = false;
            this.transform.SetParent(equipPosition);
            this.transform.localPosition = Vector3.zero;
            this.transform.localScale = Vector3.one;
            this.transform.rotation = equipPosition.rotation;

            //Disable screen wrapping since the weapon is parented.
            ScreenWrapping sw = this.transform.GetComponent<ScreenWrapping>();
            sw.enabled = false;

            equipFactor = equipPosition.localScale.x;
            inventory = inv;
            player = character;
            inventory.AddAmmo(ammoType, providedAmmo);
            providedAmmo = 0; // After the initial pickup the weapon should not supply any more ammo.
            bCanShoot = true;

            bEquipped = true;
            m_Equip.Invoke();
            return bEquipped;
        }

        public bool UnequipFromPlayer()
        {
            bool bUnequipped = false;

            this.transform.parent = null;
            collider.enabled = true;
            rigidbody.simulated = true;

            //enable screen wrapping since the weapon is no longer parented.
            ScreenWrapping sw = this.transform.GetComponent<ScreenWrapping>();
            sw.enabled = true;

            inventory = null;
            player = null;
            bCanShoot = false;

            ThrowWeapon();
            m_Unequip.Invoke();
            return bUnequipped;
        }

        private void ThrowWeapon()
        {
            Vector2 throwDirection = new Vector2(equipFactor, .25f);
            rigidbody.AddForce(throwDirection * throwSpeed);
            rigidbody.AddTorque(equipFactor * throwSpin);
        }

        public bool Fire()
        {
            bool bFired = false;

            if (bCanShoot)
            {
                int curAmmo = inventory.GetAmmo(ammoType);

                if (!eachProjectileCostAmmo)
                {
                    if (curAmmo >= projectileAmmoCost)
                    {
                        // There is enough ammo, spend the projectileAmmoCost.
                        inventory.AddAmmo(ammoType, -projectileAmmoCost);
                    }
                    else
                    {
                        // Not enough Ammo available for shot
                        m_NoAmmo.Invoke();
                        return false;
                    }
                }

                StartCoroutine(SpawnProjectiles());

                bFired = true;
                m_ShotFire.Invoke();

                // Cannot shoot again until fire rate timer has ended.
                StartCoroutine(WaitForFire());
            }


            return bFired;
        }

        private Quaternion CalculateSpread()
        {
            Quaternion spread;
            Vector3 randomVector;

            randomVector = new Vector3(Random.Range(-projectileSpreadFactor, projectileSpreadFactor), Random.Range(-projectileSpreadFactor, projectileSpreadFactor), 0f);


            spread = Quaternion.Euler(firePoint.rotation.eulerAngles + randomVector);

            return spread;
        }

        private bool InstantiateProjectile()
        {
            bool bProjectileSpawned = false;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.transform.rotation = Quaternion.RotateTowards(projectile.transform.rotation, new Quaternion(0f, 0f, Random.rotation.z, Random.rotation.w), projectileSpreadFactor);
            projectile.GetComponent<Rigidbody2D>().AddForce((projectile.transform.right * equipFactor) * projectileSpeed);

            bProjectileSpawned = true;
            m_ProjectileFire.Invoke();
            return bProjectileSpawned;
        }

        IEnumerator WaitForFire()
        {
            bCanShoot = false;
            yield return new WaitForSeconds(fireRate);
            bCanShoot = true;
        }

        IEnumerator SpawnProjectiles()
        {
            int projectilesSinceLastDelay = 0;
            for (int i = 0; i < projectilesPerShot; i++)
            {
                if (eachProjectileCostAmmo)
                {
                    int curAmmo = inventory.GetAmmo(ammoType);

                    if (curAmmo >= projectileAmmoCost)
                    {
                        // There is enough ammo, spend the projectileAmmoCost.
                        inventory.AddAmmo(ammoType, -projectileAmmoCost);
                    }
                    else
                    {
                        // Not enough Ammo available for projectile spawning\
                        m_NoAmmo.Invoke();
                        break;
                    }
                }

                if (InstantiateProjectile())
                {
                    player.ApplyKnock(-equipFactor * Vector3.right, recoilIntensity);
                    projectilesSinceLastDelay++;

                    if (projectilesSinceLastDelay >= projectilesPerDelay)
                    {
                        projectilesSinceLastDelay = 0;
                        yield return new WaitForSeconds(projectileSpawnDelay);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Interactor interactor = collision.GetComponent<Interactor>();
                interactor.equipableWeapon = this;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Interactor interactor = collision.GetComponent<Interactor>();
                if (interactor.equipableWeapon == this)
                {
                    interactor.equipableWeapon = null;
                }
            }
        }
    }
}
