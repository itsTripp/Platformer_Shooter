using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Text - Game Objects")]
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _playerLevelText;
    [SerializeField]
    private Image _rightWeaponSprite;
    [SerializeField]
    private Text _rightWeaponAmmoText;
    [SerializeField]
    private Image _leftWeaponSprite;
    [SerializeField]
    private Text _leftWeaponAmmoText;
    [Header("Health")]
    [SerializeField]
    private Image _playerHealthImage;
    [SerializeField]
    private Sprite[] _playerHealthSprites;
    [Header("Experience")]
    [SerializeField]
    private Slider _experienceSlider;
    

    private Player _player;
    private EnemySpawnManager _enemySpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is Null in UI Manager");
        }
        _enemySpawnManager = GameObject.Find("Enemy_Spawn_Manager").GetComponent<EnemySpawnManager>();
        if(_enemySpawnManager == null)
        {
            Debug.LogError("Enemy Spawn Manager is Null in UI Mananger");
        }
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);

        if(instance != null)
        {
            Debug.Log("More than one UIManager in Scene");
            return;
        }
        _experienceSlider.minValue = 0;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoCount();
        _experienceSlider.maxValue = GameControl.gameControl.experienceToNextLevel;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateHealth (int currentHealth)
    {
        _playerHealthImage.sprite = _playerHealthSprites[currentHealth];
        if(currentHealth == 0)
        {
            _gameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerEffect());
        }
    }

    IEnumerator GameOverFlickerEffect()
    {
        while(true)
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
        _experienceSlider.value = GameControl.gameControl.playerXP;
        if (GameControl.gameControl.playerXP >= GameControl.gameControl.experienceToNextLevel)
        {
            SetLevel(GameControl.gameControl.playerLevel + 1);
            return true;
        }
        UpdateLevel();
        return false;
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
        if(GameControl.gameControl.currentLeftWeapon != null)
        {
            _leftWeaponSprite.sprite = GameControl.gameControl.currentLeftWeapon.currentWeaponSprite;
            _leftWeaponSprite.color = new Color(_leftWeaponSprite.color.r,
                _leftWeaponSprite.color.g, _leftWeaponSprite.color.b, 1f);
        }
        if(GameControl.gameControl.currentRightWeapon != null)
        {
            _rightWeaponSprite.sprite = GameControl.gameControl.currentRightWeapon.currentWeaponSprite;
            _rightWeaponSprite.color = new Color(_rightWeaponSprite.color.r, 
                _rightWeaponSprite.color.g, _rightWeaponSprite.color.b, 1f);
        }
    }
    public void UpdateAmmoCount()
    {
        if(GameControl.gameControl.currentLeftWeapon != null)
        {
            _leftWeaponAmmoText.text = GameControl.gameControl.leftWeaponCurrentAmmo + "/" +
                GameControl.gameControl.currentLeftWeapon.maximumAmmo;
            _leftWeaponAmmoText.color = new Color(_leftWeaponAmmoText.color.r, 
                _leftWeaponAmmoText.color.g, _leftWeaponAmmoText.color.b, 1f);
        }
        if(GameControl.gameControl.currentRightWeapon != null)
        {
            _rightWeaponAmmoText.text = GameControl.gameControl.rightWeaponCurrentAmmo + "/" +
            GameControl.gameControl.currentRightWeapon.maximumAmmo;
            _rightWeaponAmmoText.color = new Color(_rightWeaponAmmoText.color.r,
                _rightWeaponAmmoText.color.g, _rightWeaponAmmoText.color.b, 1f);
        }
    }
}
