using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EpicTortoiseStudios
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager UIManagerInstance;

        [Header("Text - Game Objects")]
        [SerializeField]
        private TextMeshProUGUI _healthText;
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        [SerializeField]
        private TextMeshProUGUI _gameOverText;
        [SerializeField]
        public TextMeshProUGUI _waveText;
        [SerializeField]
        private TextMeshProUGUI _playerLevelText;
        [SerializeField]
        private Image _rightWeaponSprite;
        [SerializeField]
        private TextMeshProUGUI _rightWeaponAmmoText;
        [SerializeField]
        private Image _leftWeaponSprite;
        [SerializeField]
        private TextMeshProUGUI _leftWeaponAmmoText;
        //[SerializeField]
        //private TextMeshProUGUI _pistolAmmoText;
        //[SerializeField]
        //private TextMeshProUGUI _shotgunAmmoText;
        [Header("Health")]
        [SerializeField]
        private Image _playerHealthImage;
        [SerializeField]
        private Sprite[] _playerHealthSprites;
        [Header("Experience")]
        /*[SerializeField]
        private Slider _experienceSlider*/
        [SerializeField]
        private Image _experienceBar;
        private float fillSmoothness = 0.001f;

        public const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;
        [SerializeField]
        private Image healthBarImage;
        [SerializeField]
        private Image damagedHealthBarImage;
        public float damagedHealthShrinkTimer;


        private Player _player;
        private EnemySpawnManager _enemySpawnManager;

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player == null)
            {
                Debug.LogError("Player is Null in UI Manager");
            }
            _enemySpawnManager = GameObject.Find("Enemy_Spawn_Manager").GetComponent<EnemySpawnManager>();
            if (_enemySpawnManager == null)
            {
                Debug.LogError("Enemy Spawn Manager is Null in UI Mananger");
            }
            _scoreText.text = "Score: " + 0;
            _gameOverText.gameObject.SetActive(false);

            /*if (UIManagerInstance != null)
            {
                Debug.Log("More than one UIManager in Scene");
                return;
            }*/
            if(UIManagerInstance == null)
            {
                UIManagerInstance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            //_experienceSlider.minValue = 0;
            _experienceBar.fillAmount = 0;
            DontDestroyOnLoad(gameObject);

            //GameControl.gameControl = new GameControl(5);
            SetHealth(GameControl.gameControl.GetHealthNormalized());
            damagedHealthBarImage.fillAmount = healthBarImage.fillAmount;

            GameControl.gameControl.OnDamaged += HealthSystem_OnDamaged;
            GameControl.gameControl.OnHealed += HealthSystem_OnHealed;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAmmoCount();
            FillExperience();
            ShrinkHealth();
            //_experienceSlider.maxValue = GameControl.gameControl.experienceToNextLevel;
            
            //_experienceBar.fillAmount = (float)GameControl.gameControl.playerXP / (float)GameControl.gameControl.experienceToNextLevel * Time.deltaTime;
            //_experienceBar.fillAmount += _xpFillSpeed * Time.deltaTime;
            /*if(PauseMenu.pauseInstance.gameObject.activeSelf == true)
            {
                if(_waveText.gameObject.activeSelf == true)
                {
                    _waveText.gameObject.SetActive(false);
                }
            */
        }

        public void UpdateScore(int playerScore)
        {
            _scoreText.text = "Score: " + playerScore;
        }

        /*public void UpdateHealth(int currentHealth)
        {
            _playerHealthImage.sprite = _playerHealthSprites[currentHealth];
            if (currentHealth == 0)
            {
                _gameOverText.gameObject.SetActive(true);
                StartCoroutine(GameOverFlickerEffect());
            }
        }*/

        IEnumerator GameOverFlickerEffect()
        {
            while (true)
            {
                _gameOverText.text = "Game Over";
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void SpawnNextWave()
        {
            StartCoroutine(EnableWaveText());
        }

        IEnumerator EnableWaveText()
        {
            _waveText.text = "Wave " + _enemySpawnManager.GetWaveNumber() + " / " +
                _enemySpawnManager.maxWaveNumber;
            _waveText.gameObject.SetActive(true);
            _enemySpawnManager.EnableNextWaveSpawn();
            yield return new WaitForSeconds(3f);
            _waveText.gameObject.SetActive(false);
        }

        public bool AddExperience(int expereienceToAdd)
        {
            GameControl.gameControl.playerXP += expereienceToAdd;
            //_experienceSlider.value = GameControl.gameControl.playerXP;
            if (GameControl.gameControl.playerXP >= GameControl.gameControl.experienceToNextLevel)
            {
                SetLevel(GameControl.gameControl.playerLevel + 1);
                return true;
            }
            UpdateLevel();
            return false;
        }

        private void FillExperience()
        {
            float prevFill = _experienceBar.fillAmount;
            float currFill = (float)GameControl.gameControl.playerXP / (float)GameControl.gameControl.experienceToNextLevel;
            if (currFill > prevFill)
            {
                prevFill = Mathf.Min(prevFill + fillSmoothness, currFill);
            }
            else if (currFill < prevFill)
            {
                prevFill = Mathf.Max(prevFill - fillSmoothness * 10, currFill);
            }
            _experienceBar.fillAmount = prevFill;
        }

        

        private void SetLevel(int value)
        {
            GameControl.gameControl.playerLevel = value;
            GameControl.gameControl.playerXP = GameControl.gameControl.playerXP -
                GameControl.gameControl.experienceToNextLevel;

            GameControl.gameControl.experienceToNextLevel = (int)(50f *
                (Mathf.Pow(GameControl.gameControl.playerLevel + 1, 2) -
                (5 * (GameControl.gameControl.playerLevel + 1)) + 8));
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            _playerLevelText.text = GameControl.gameControl.playerLevel.ToString();
        }

        public void UpdateWeapon()
        {
            if (GameControl.gameControl.currentLeftWeapon != null)
            {
                _leftWeaponSprite.sprite = GameControl.gameControl.currentLeftWeapon.currentWeaponSprite;
                _leftWeaponSprite.color = new Color(_leftWeaponSprite.color.r,
                    _leftWeaponSprite.color.g, _leftWeaponSprite.color.b, 1f);
            }
            if (GameControl.gameControl.currentRightWeapon != null)
            {
                _rightWeaponSprite.sprite = GameControl.gameControl.currentRightWeapon.currentWeaponSprite;
                _rightWeaponSprite.color = new Color(_rightWeaponSprite.color.r,
                    _rightWeaponSprite.color.g, _rightWeaponSprite.color.b, 1f);
            }
        }
        public void UpdateAmmoCount()
        {
            if (GameControl.gameControl.currentLeftWeapon != null)
            {
                if(GameControl.gameControl.currentLeftWeapon.weaponType == Weapon.WeaponType.Pistol)
                {
                    _leftWeaponAmmoText.text = GameControl.gameControl.pistolAmmo.ToString();
                    _leftWeaponAmmoText.color = new Color(_leftWeaponAmmoText.color.r,
                        _leftWeaponAmmoText.color.g, _leftWeaponAmmoText.color.b, 1f);
                }
                if (GameControl.gameControl.currentLeftWeapon.weaponType == Weapon.WeaponType.Shotgun)
                {
                    _leftWeaponAmmoText.text = GameControl.gameControl.shotgunAmmo.ToString();
                    _leftWeaponAmmoText.color = new Color(_leftWeaponAmmoText.color.r,
                        _leftWeaponAmmoText.color.g, _leftWeaponAmmoText.color.b, 1f);
                }

            }
            if (GameControl.gameControl.currentRightWeapon != null)
            {
                if (GameControl.gameControl.currentRightWeapon.weaponType == Weapon.WeaponType.Pistol)
                {
                    _rightWeaponAmmoText.text = GameControl.gameControl.pistolAmmo.ToString();
                    _rightWeaponAmmoText.color = new Color(_rightWeaponAmmoText.color.r,
                        _rightWeaponAmmoText.color.g, _rightWeaponAmmoText.color.b, 1f);
                }
                if (GameControl.gameControl.currentRightWeapon.weaponType == Weapon.WeaponType.Shotgun)
                {
                    _rightWeaponAmmoText.text = GameControl.gameControl.shotgunAmmo.ToString();
                    _rightWeaponAmmoText.color = new Color(_rightWeaponAmmoText.color.r,
                        _rightWeaponAmmoText.color.g, _rightWeaponAmmoText.color.b, 1f);
                }
            }
        }

        private void HealthSystem_OnHealed(object sender, System.EventArgs e)
        {
            SetHealth(GameControl.gameControl.GetHealthNormalized());
            damagedHealthBarImage.fillAmount = healthBarImage.fillAmount;
        }

        private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
        {
            damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
            SetHealth(GameControl.gameControl.GetHealthNormalized());
            Debug.Log("OnDamaged Is Fired");
        }

        public void SetHealth(float healthNormalized)
        {
            healthBarImage.fillAmount = healthNormalized;
            _healthText.text = GameControl.gameControl.playerCurrentHealth.ToString() + " / " + GameControl.gameControl.playerMaxHealth.ToString();

            if (GameControl.gameControl.playerCurrentHealth == 0)
            {
                _gameOverText.gameObject.SetActive(true);
                StartCoroutine(GameOverFlickerEffect());
            }
        }

        private void ShrinkHealth()
        {
            damagedHealthShrinkTimer -= Time.deltaTime;
            if (damagedHealthShrinkTimer < 0)
            {
                if (healthBarImage.fillAmount < damagedHealthBarImage.fillAmount)
                {
                    float shrinkSpeed = 1f;
                    damagedHealthBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
                }
            }
        }

        public void Heal()
        {
            GameControl.gameControl.Heal(1);
        }
        public void Damage()
        {
            GameControl.gameControl.Damage(1);
        }
    }
}
