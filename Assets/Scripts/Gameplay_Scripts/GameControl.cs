using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;

namespace EpicTortoiseStudios
{
    public class GameControl : MonoBehaviour
    {
        public static GameControl gameControl;

        public event EventHandler OnDamaged;
        public event EventHandler OnHealed;

        [Header("Player Health")]
        public int playerMaxHealth = 5;
        public int playerCurrentHealth;

        [Header("Player Weapon Info")]
        public Weapon currentRightWeapon;
        public Weapon currentLeftWeapon;
        public int rightWeaponCurrentAmmo;
        public int leftWeaponCurrentAmmo;

        [Header("Player Experience")]
        [SerializeField]
        private int _playerXP = 0;
        public int playerXP
        {
            get { return _playerXP; }
            set
            {
                _playerXP = value;
            }
        }
        public int experienceToNextLevel;
        [SerializeField]
        private int _playerLvl = 0;
        public int playerLevel
        {
            get { return _playerLvl; }
            set
            {
                _playerLvl = value;
                if (onLevelChange != null)
                {
                    onLevelChange();
                    ShowData("level");
                }
            }
        }

        public TextMeshProUGUI txtPlayerLevel;
        public TextMeshProUGUI txtPlayerXP;

        [Header("Entire Perk List")]
        public List<Perks> possiblePerks = new List<Perks>();

        [Header("Enabled Perks")]
        public List<Perks> m_PlayerPerks = new List<Perks>();
        public List<Perks> PlayerPerks
        {
            get { return m_PlayerPerks; }
            set
            {
                m_PlayerPerks = value;
                if (onPerkChange != null)
                {
                    onPerkChange();
                }
            }
        }

        private void Start()
        {
            ShowData("level");
            possiblePerks = Resources.LoadAll<Perks>("Perks").ToList();

            /*DontDestroyOnLoad(gameObject);
            if(gameControl == null)
            {
                gameControl = this;
            }
            else
            {
                Destroy(gameObject);
            }*/
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (gameControl == null)
            {
                gameControl = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
           
        }

        public void ShowData(string i)
        {
            if (i == "level")
            {
                txtPlayerLevel.text = _playerLvl.ToString();
            }
        }

        public delegate void OnPerkChange();
        public event OnPerkChange onPerkChange;

        public delegate void OnLevelChange();
        public event OnLevelChange onLevelChange;

        public void UpdateLevel(int amount)
        {
            playerLevel += amount;
            Debug.Log("Level is Firing");
        }

        public GameControl(int healthAmount)
        {
            playerMaxHealth = healthAmount;
            playerCurrentHealth = healthAmount;
        }

        public void Damage(int amount)
        {
            playerCurrentHealth -= amount;
            if (playerCurrentHealth < 0)
            {
                playerCurrentHealth = 0;
            }
            if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
        }

        public void Heal(int amount)
        {
            playerCurrentHealth += amount;
            if (playerCurrentHealth > playerMaxHealth)
            {
                playerCurrentHealth = playerMaxHealth;
            }
            if (OnHealed != null) OnHealed(this, EventArgs.Empty);
        }

        public float GetHealthNormalized()
        {
            return (float)playerCurrentHealth / playerMaxHealth;
        }
    }
}
