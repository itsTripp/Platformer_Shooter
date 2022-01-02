using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EpicTortoiseStudios
{
    public class HealthSystem : MonoBehaviour
    {

        public event EventHandler OnDamaged;
        public event EventHandler OnHealed;
        private int healthAmount;
        private int healthAmountMax;

        public HealthSystem(int healthAmount)
        {
            GameControl.gameControl.playerMaxHealth = healthAmount;
            GameControl.gameControl.playerCurrentHealth = healthAmount;
        }

        public void Damage(int amount)
        {
            GameControl.gameControl.playerCurrentHealth -= amount;
            if(GameControl.gameControl.playerCurrentHealth < 0)
            {
                GameControl.gameControl.playerCurrentHealth = 0;
            }
            if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
        }

        public void Heal(int amount)
        {
            GameControl.gameControl.playerCurrentHealth += amount;
            if(GameControl.gameControl.playerCurrentHealth > GameControl.gameControl.playerMaxHealth)
            {
                GameControl.gameControl.playerCurrentHealth = GameControl.gameControl.playerMaxHealth;
            }
            if (OnHealed != null) OnHealed(this, EventArgs.Empty);
        }

        public float GetHealthNormalized()
        {
            return (float)GameControl.gameControl.playerCurrentHealth / GameControl.gameControl.playerMaxHealth;
        }
    }
}
