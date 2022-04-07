using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EpicTortoiseStudios
{
    public class SpawnPool : MonoBehaviour
    {
        [System.Serializable]
        public class EnemySpawn
        {
            [Tooltip("Prefab GameObject that will be spawned.")]
            [SerializeField] public GameObject _enemyPrefab;
            [Tooltip("How many tickets does this enemy cost to spawn?")]
            [SerializeField] [Min(0)] public int _ticketCost = 1;
            [Tooltip("What percent chance does the enemy have to spawn? All Enemy Chance Percent should equal up to 100%.")]
            [SerializeField] [Range(0, 100)] public float _chancePercent = 100f;
        }

        [TextArea(0,10)]
        public string _ComponentDescription = string.Format("The Spawn Pool acts as a local manager for spawning enemies in an arena/area of the level or throughout the entirety of the level.\n" +
            " Spawn Pools consist of a ticket based degredation. This means that when an enemy is spawned, they will remove a ticket(s) from the pool.\n" +
            " Draining the tickets from the pool will effectively 'Defeat' the Spawn Pool, preventing any further enemies from spawning.\n" +
            " Spawn Pools can also have a timer that will deactivate the Spawn Pool when the timer runs out. This can work alongside the ticket pool, or without the ticket pool.\n" +
            " The Spawn Pool also tracks all active enemies and declares when it cannot spawn any more enemies and all enemies have been defeated.");
        
        [Space(30)]
        [Header("Spawn Mechanics")]
        [Tooltip("Minimum Tickets that will be randomly selected as the spawn pools ticket count.")]
        [SerializeField] [Min(0)] int _minTickets = 0;
        [Tooltip("Maximum Tickets that will be randomly selected as the spawn pools ticket count.")]
        [SerializeField] [Min(0)] int _maxTickets = 0;
        [Tooltip("Amount of time (in seconds) the spawner remains active. (Set to 0 if spawner shouldnt use a timer.)")]
        [SerializeField] [Min(0)] float _timer = 0;
        [Tooltip("Min time (in seconds) between enemy spawns.")]
        [SerializeField] [Min(0)] float _minSpawnTime = 0;
        [Tooltip("Max time (in seconds) between enemy spawns.")]
        [SerializeField] [Min(0)] float _maxSpawnTime = 0;
        [Tooltip("Maximum Active Tickets at a time. (5 Tickets would allow 5 1 ticket enemies to be spawned at a time. Setting to 0 disables.)")]
        [SerializeField] [Min(0)] int _maxSpawnedTickets = 0;
        [Tooltip("Maximum Spawned Enemies at a time. (Setting to 0 disables.)")]
        [SerializeField] [Min(1)] int _maxSpawnedEnemies = 0;

        [Space(10)]
        [SerializeField] List<EnemySpawn> _enemyList = new List<EnemySpawn>();


        [Header("Events")]
        [SerializeField] UnityEvent m_SpawnPoolActivated;
        [SerializeField] UnityEvent m_SpawnPoolDeactivated;
        [SerializeField] UnityEvent<float> m_TimerUpdate;
        [SerializeField] UnityEvent<int> m_TicketsUpdate;
        [SerializeField] UnityEvent m_EnemySpawned;

        private int _actTicketMax = 0;
        private int _curTickets = 0;

        private float _actTimeToSpawn = 0f;
        private float _curTimeToSpawn = 0f;

        private float _totalSpawnPercent = 0f;

        private bool _active = false;
        private bool _useTimer = false;
        private float _activeTime = 0f;
        private int _enemiesSpawned = 0;
        private int _activeTickets = 0;
        private List<GameObject> _activeEnemies = new List<GameObject>();

        private GameObject _spawnedObject;
        private GameObject _spawnPointsObject;

        private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

        private void Start()
        {
            _spawnedObject = this.transform.GetChild(0).gameObject;
            _spawnPointsObject = this.transform.GetChild(1).gameObject;

            Activate();
        }

        private void FixedUpdate()
        {
            if (_active)
            {
                _activeTime += Time.deltaTime;
                _curTimeToSpawn += Time.deltaTime;

                if (_useTimer && _activeTime >= _timer)
                {
                    //Deactivate Spawn Pool
                    DeActivate();
                    return;
                }

                if (_curTickets > 0 || _actTicketMax == 0)
                {
                    if (_curTimeToSpawn >= _actTimeToSpawn)
                    {
                        ChooseAndSpawn();
                    }
                } else
                {
                    DeActivate();
                    return;
                }
            }
        }

        public void Activate()
        {
            _activeTime = 0f;
            _actTicketMax = Random.Range(_minTickets, _maxTickets);
            _curTickets = _actTicketMax;

            if (_timer > 0)
            {
                _useTimer = true;
            }

            _spawnPoints = _spawnPointsObject.GetComponentsInChildren<SpawnPoint>(false).ToList();

            _active = true;
            _actTimeToSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
            _curTimeToSpawn = 0f;

            GetTotalAvailablePercent();
        }

        public void DeActivate()
        {
            _active = false;
        }

        private void GetTotalAvailablePercent()
        {
            _totalSpawnPercent = 0f;
            foreach (EnemySpawn enemy in _enemyList)
            {
                if (_curTickets >= enemy._ticketCost)
                {
                    if (_activeTickets + enemy._ticketCost <= _actTicketMax)
                    {
                        _totalSpawnPercent += enemy._chancePercent;
                    }
                }
            }
        }

        public EnemySpawn GetSpawnObject(out int spawnedTicketCost)
        {
            EnemySpawn enemy = null;
            int loopTimeMax = 10;
            int loopIndex = 0;

            GetTotalAvailablePercent();

            while (enemy == null || loopIndex < loopTimeMax)
            {
                loopIndex++;
                int enemyIndex = Random.Range(0, _enemyList.Count);
                EnemySpawn tryEnemy = _enemyList[enemyIndex];

                if (_curTickets - tryEnemy._ticketCost >= 0)
                {
                    if (_activeTickets + tryEnemy._ticketCost <= _actTicketMax)
                    {
                        enemy = tryEnemy;
                    }
                }

                if (_curTickets <= 0)
                {
                    break;
                }
            }

            spawnedTicketCost = enemy._ticketCost;
            return enemy;
        }

        public EnemySpawn GetRandomEnemyOnPercent(out int spawnedTicketCost)
        {
            if (_curTickets <= 0)
            {
                spawnedTicketCost = 0;
                return null;
            }

            //Generate a random position in the list.
            float pick = Random.value * _totalSpawnPercent;
            int chosenIndex = 0;
            float cumalativeChance = _enemyList[0]._chancePercent;

            //Step through the list until we've accumulated more weight than this.
            //The length check is for safety in case rounding errors accumulate.
            while (pick > cumalativeChance && chosenIndex < _enemyList.Count - 1)
            {
                chosenIndex++;
                if (_curTickets >= _enemyList[chosenIndex]._ticketCost)
                {
                    if (_activeTickets + _enemyList[chosenIndex]._ticketCost <= _actTicketMax)
                    {
                        cumalativeChance += _enemyList[chosenIndex]._chancePercent;
                    }
                }
            }

            // Spawn the chosen item.
            spawnedTicketCost = _enemyList[chosenIndex]._ticketCost;
            return _enemyList[chosenIndex];
        }

        public SpawnPoint GetSpawnPoint()
        {
            SpawnPoint spawn = null;

            while (spawn == null)
            {
                int spawnIndex = Random.Range(0, _spawnPoints.Count);
                SpawnPoint trySpawn = _spawnPoints[spawnIndex];

                if (trySpawn._active)
                {
                    spawn = trySpawn;
                }
            }

            return spawn;
        }

        public void SpawnEnemy(EnemySpawn enemy, SpawnPoint spawn)
        {
            //1. Get Location of Spawn
            Vector3 spawnPos = spawn.transform.position;
            Quaternion spawnRot = spawn.transform.rotation;

            //2. Instance prefab at spawn transform
            GameObject instance = Instantiate(enemy._enemyPrefab, spawnPos, spawnRot, _spawnedObject.transform);
            SpawnedObject spawnObjectRef = instance.AddComponent<SpawnedObject>();
            spawnObjectRef._spawnPool = this;
            spawnObjectRef._ticketCost = enemy._ticketCost;
            _activeEnemies.Add(instance);
            _activeTickets += enemy._ticketCost;
        }

        public void ChooseAndSpawn()
        {
            if (_activeEnemies.Count < _maxSpawnedEnemies)
            {
                int spawnedTicketCost = 0;
                EnemySpawn spawnObject = GetRandomEnemyOnPercent(out spawnedTicketCost);
                if (spawnObject != null)
                {
                    SpawnEnemy(spawnObject, GetSpawnPoint());
                } else
                {
                    return;
                }
                _curTickets -= spawnedTicketCost;
                //Debug.Log("Remaining Pool Tickets: " + _curTickets.ToString());
                _actTimeToSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
                _curTimeToSpawn = 0f;
            }
        }

        public void EnemyDestroyed(GameObject enemy, int ticketCost)
        {
            _activeEnemies.Remove(enemy);
            _activeTickets -= ticketCost;
            //Debug.Log("Remaining Active Tickets: " + _activeTickets.ToString());
        }
    }
    
}