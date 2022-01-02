using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicTortoiseStudios
{
    public class HealthBar : MonoBehaviour
    {
        private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;

        private Image barImage;
        private Image damagedBarImage;
        private float damagedHealthShrinkTimer;
        private HealthSystem healthSystem;
        private void Awake()
        {
            barImage = transform.Find("Bar").GetComponent<Image>();
            damagedBarImage = transform.Find("DamagedBar").GetComponent<Image>();
        }

        private void Start()
        {
            healthSystem = new HealthSystem(5);
            SetHealth(healthSystem.GetHealthNormalized());
            damagedBarImage.fillAmount = barImage.fillAmount;

            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnHealed += HealthSystem_OnHealed;
        }

        private void Update()
        {
            damagedHealthShrinkTimer -= Time.deltaTime;
            if (damagedHealthShrinkTimer < 0)
            {
                if(barImage.fillAmount < damagedBarImage.fillAmount)
                {
                    float shrinkSpeed = 1f;
                    damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime; 
                }
            }
        }

        private void HealthSystem_OnHealed(object sender, System.EventArgs e)
        {
            SetHealth(healthSystem.GetHealthNormalized());
            damagedBarImage.fillAmount = barImage.fillAmount;
        }

        private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
        {
            damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
            SetHealth(healthSystem.GetHealthNormalized());
            
        }

        private void SetHealth(float healthNormalized)
        {
            barImage.fillAmount = healthNormalized;
        }

        public void Heal()
        {
            healthSystem.Heal(1);
        }
        public void Damage()
        {
            healthSystem.Damage(1);
        }
    }
}
