using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameActive = true;

    [Header("Enemy Objects")]
    [SerializeField]
    private GameObject[] _enemy;
    [SerializeField]
    private GameObject _enemyContainer;

    public int[] enemyTable =
    {
        100
    };

    private int _enemyTotalWeight;
    private int _enemyRandomNumber;
    [Header("Enemy Spawn")]
    [SerializeField]
    private Transform[] _enemySpawnPoints;
    [SerializeField]
    private float _enemySpawnRate = 5f;
    private bool _stopSpawningEnemies = false;
    [SerializeField]
    private int _currentEnemies;
    [SerializeField]
    private int _maxEnemies; 
        
    [Header("Enemy Waves")]
    [SerializeField]
    private bool _spawnEnemyWave = false;
    [SerializeField]
    private int _enemiesInCurrentWave = 15;
    [SerializeField]
    private int _currentWaveNumber = 1;
    public int maxWaveNumber;
    
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is Null in Enemy Spawn Manager");
        }
        foreach(var item in enemyTable)
        {
            _enemyTotalWeight += item;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentEnemies <= 0 && _spawnEnemyWave == false)
        {
            EnableNextWaveSpawn();
            _uiManager.SpawnNextWave();
            StartEnemySpawning();
        }
        if(_isGameActive == false)
        {
            StopCoroutine(SpawnEnemy());
        }
    }

    public void StartSpawningEnemies()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
            yield return new WaitForSeconds(3f);
        while (_isGameActive == true && _spawnEnemyWave == true)
        {
            for (int i = 0; i < _enemiesInCurrentWave; i++)
            {
                ChooseEnemy();
                _currentEnemies++;
                yield return new WaitForSeconds(3f);

                if(_isGameActive == false)
                {
                    break;
                }
            }
            _enemiesInCurrentWave += 3;
            _currentWaveNumber++;
            _spawnEnemyWave = false;
        }                    
    }

    public void StartEnemySpawning()
    {
        if (_currentWaveNumber % 2 == 0)
        {
            _enemySpawnRate -= 0.2f;
            if(_enemySpawnRate <= 0.4f)
            {
                _enemySpawnRate = 0.4f;
            }
        }
        StartCoroutine(SpawnEnemy());
    }

    private void ChooseEnemy()
    {
        _enemyRandomNumber = Random.Range(0, _enemyTotalWeight);
        Debug.Log("Enemy Random Number: " + _enemyRandomNumber);
        for (int i = 0; i < enemyTable.Length; i++)
        {
            if (_enemyRandomNumber <= enemyTable[i])
            {
                Transform spawnPointReference = GetSpawnPointReference();                
                GameObject newEnemy = Instantiate(_enemy[i], spawnPointReference.position, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                return;
            }
            else
            {
                _enemyRandomNumber -= enemyTable[i];
            }
        }
    }

    private Transform GetSpawnPointReference()
    {
        int randomIndex = Random.Range(0, _enemySpawnPoints.Length);
        return _enemySpawnPoints[randomIndex];
    }

    public void EnemyKilled()
    {
        _currentEnemies--;
    }

    public int GetWaveNumber()
    {
        return _currentWaveNumber;
    }

    public void EnableNextWaveSpawn()
    {
        _spawnEnemyWave = true;
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemies = true;
    }

    public void GameOver()
    {
        _stopSpawningEnemies = true;
        Debug.Log("GameOver");
    }
}
