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
    private Text _rightWeaponAmmoText;
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
    [SerializeField]
    private int _playerLevel = 0;
    [SerializeField]
    private int _playerExperience;
    [SerializeField]
    private int _experienceToNextLevel;



    private Player _player;
    [SerializeField]
    //private Weapon _weapon;
    private EnemySpawnManager _enemySpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is Null in UI Manager");
        }
        /*_weapon = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
        if(_weapon == null)
        {
            Debug.LogError("Weapon is Null in UI Manager");
        }*/
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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoCount();
        _experienceSlider.maxValue = _experienceToNextLevel;
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
        _waveText.text = "Wave " + _enemySpawnManager.GetWaveNumber() + " / " + _enemySpawnManager.maxWaveNumber;
        _waveText.gameObject.SetActive(true);
        _enemySpawnManager.EnableNextWaveSpawn();
        yield return new WaitForSeconds(3f);
        _waveText.gameObject.SetActive(false);
    }

    public bool AddExperience(int expereienceToAdd)
    {
        _playerExperience += expereienceToAdd;
        _experienceSlider.value = _playerExperience;
        if(_playerExperience >= _experienceToNextLevel)
        {
            SetLevel(_playerLevel + 1);
            return true;
        }
        UpdateLevel();
        return false;
    }

    private void SetLevel(int value)
    {
        _playerLevel = value;
        _playerExperience = _playerExperience - _experienceToNextLevel;
        _experienceToNextLevel = (int)(50f * (Mathf.Pow(_playerLevel + 1, 2) - (5 * (_playerLevel + 1)) + 8));
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        _playerLevelText.text = _playerLevel.ToString();
    }

    private void UpdateWeapon()
    {

    }
    public void UpdateAmmoCount()
    {
        /*_rightWeaponAmmoText.text = _weapon._currentAmmo + "/" + _weapon._maximumAmmo;
        _leftWeaponAmmoText.text = _weapon._currentAmmo + "/" + _weapon._maximumAmmo;*/
    }
}
