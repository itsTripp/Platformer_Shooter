using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace EpicTortoiseStudios
{

    public class Health : MonoBehaviour
    {

        [SerializeField]
        private float maxHealth = 100f;
        [SerializeField]
        private bool applyKnockback = true;
        [SerializeField]
        private bool displayDamagePopup = true;
        [SerializeField] private Transform pfDamagePopup;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [SerializeField] UnityEvent<float, float> m_HealthIncreased;
        [SerializeField] UnityEvent<float, float> m_HealthDecreased;
        [SerializeField] UnityEvent m_HealthEmpty;

        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DamageRadius outDamageRad;
            Damage outDamage;
            if (collision.transform.TryGetComponent(out outDamageRad))
            {
                DamageRadius[] damages = collision.transform.GetComponents<DamageRadius>();
                float totalDamage = 0f;
                float totalKnock = 0f;

                float appliedDamage = ResistDamage(damages, out totalDamage, out totalKnock);
                ApplyDamage(appliedDamage);

                float damageAppliedPercent = (appliedDamage / totalDamage) * 100;

                if (appliedDamage > 0)
                {
                    ApplyKnockback(damageAppliedPercent * totalKnock, collision.transform.position);
                }
            } else if (collision.transform.TryGetComponent(out outDamage))
            {
                //Get all damage components on collision
                Damage[] damages = collision.transform.GetComponents<Damage>();
                float totalDamage = 0f;
                float totalKnock = 0f;
                float appliedDamage = ResistDamage(damages, out totalDamage, out totalKnock);
                ApplyDamage(appliedDamage);

                float damageAppliedPercent = (appliedDamage / totalDamage) * 100;

                if (appliedDamage > 0)
                {
                    ApplyKnockback(damageAppliedPercent * totalKnock, collision.transform.position);
                }
                
            }
        }

        private float ResistDamage(Damage[] damages, out float totalDamage, out float totalKnock)
        {
            //Get local resistances, and compare against damaged type of Damage object.
            //A resistance will have a damage type and a resistance percent.
            //I.E Character has .25 resistance against pistol damage, pistol damage is 10, character takes 7.5 damage.

            float damage = 0;

            totalDamage = 0;
            totalKnock = 0;

            DamageResistance[] damageResistances = this.GetComponents<DamageResistance>();

            foreach (Damage dmg in damages)
            {
                float newDamage = dmg.damage;
                foreach (DamageResistance resist in damageResistances)
                {
                    
                    if (dmg.type == resist.type)
                    {
                        //Can resist damage
                        newDamage = dmg.damage * (1 - resist.percent);
                        break;
                    }
                }

                damage += newDamage;
                totalDamage += dmg.damage;
                totalKnock += dmg.knock;
            }

            return damage;
        }

        private float ResistDamage(DamageRadius[] damages, out float totalDamage, out float totalKnock)
        {
            //Get local resistances, and compare against damaged type of Damage object.
            //A resistance will have a damage type and a resistance percent.
            //I.E Character has .25 resistance against pistol damage, pistol damage is 10, character takes 7.5 damage.

            float damage = 0;

            totalDamage = 0;
            totalKnock = 0;

            DamageResistance[] damageResistances = this.GetComponents<DamageResistance>();

            foreach (DamageRadius dmg in damages)
            {
                //Get distance between this and the origin of the damage object.
                float distance = Vector3.Distance(this.transform.position, dmg.transform.position);

                float newDamage = dmg.damage;
                foreach (DamageResistance resist in damageResistances)
                {

                    if (dmg.type == resist.type)
                    {
                        //Can resist damage
                        newDamage = dmg.damage * (1 - resist.percent);
                        break;
                    }
                }

                damage += newDamage;
                totalDamage += dmg.damage;
                totalKnock += dmg.knock;
            }

            return damage;
        }

        private void ApplyDamage(float damage)
        {
            float newHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            float damageTaken = currentHealth - newHealth;

            if (newHealth > currentHealth)
            {
                //Health gained
                m_HealthIncreased.Invoke(newHealth, damageTaken);
                DisplayDamageUI(damageTaken);
            }
            else if (newHealth < currentHealth)
            {
                //Damage taken
                m_HealthDecreased.Invoke(newHealth, damageTaken);
                DisplayDamageUI(damageTaken);
            }

            currentHealth = newHealth;

            if (currentHealth == 0)
            {
                m_HealthEmpty.Invoke();
            }
        }

        private void ApplyKnockback(float knockBack, Vector3 position)
        {
            if (!applyKnockback) return;

            Vector3 direction = transform.position - position;
            _rigidbody2D.AddForceAtPosition(direction * knockBack, direction);
        }

        private void DisplayDamageUI(float damageTaken)
        {
            if (!displayDamagePopup) return;

            Transform damagePopupTransform = Instantiate(pfDamagePopup, transform.position, Quaternion.identity);
            DamagePopup dmgPop = damagePopupTransform.GetComponent<DamagePopup>();

            float percOfMax = (damageTaken / maxHealth) * 100;

            dmgPop.Setup(damageTaken, percOfMax);
        }

        public void AddResistance(DamageResistance newResist)
        {
            AddResistance(newResist.type, newResist.percent, newResist.canOverResist);
        }

        public void AddResistance(CommonEnums.DamageType type, float percent, bool canOverResist = false)
        {
            DamageResistance newDmgResist = new DamageResistance(type, percent, canOverResist);

            DamageResistance[] damageResistances = this.GetComponents<DamageResistance>();

            foreach (DamageResistance resist in damageResistances)
            {
                if (resist.type == type)
                {
                    //Resistance already exists, add percent
                    resist.canOverResist = canOverResist;

                    if (resist.canOverResist)
                    {
                        resist.percent += percent;
                    }
                    else
                    {
                        resist.percent = Mathf.Clamp(resist.percent + percent, -1, 1);
                    }
                }
                else
                {
                    DamageResistance newComponent = gameObject.AddComponent<DamageResistance>();
                    newComponent.canOverResist = newDmgResist.canOverResist;
                    newComponent.percent = newDmgResist.percent;
                    newComponent.type = newDmgResist.type;
                }
            }
        }

        public void RemoveResistance(DamageResistance resistToRemove)
        {
            RemoveResistance(resistToRemove.type, resistToRemove.percent, resistToRemove.canOverResist);
        }

        public void RemoveResistance(CommonEnums.DamageType type, float percent, bool canOverResist = false)
        {
            DamageResistance[] damageResistances = this.GetComponents<DamageResistance>();

            foreach (DamageResistance resist in damageResistances)
            {
                if (resist.type == type)
                {
                    resist.percent -= percent;
                    if (resist.percent == 0)
                    {
                        Destroy(resist);
                        break;
                    }
                }
            }
        }
    }
}

