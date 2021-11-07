using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChestSpawner : MonoBehaviour
{
    public Transform[] lootSpawnPoints;
    public float spawnTime = 1.5f;
    public GameObject[] lootToSpawn;

    public List<Transform> possibleLootSpawns = new List<Transform>();

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is Null on Loot Chest Spawner");
        }
        for(int i = 0; i < lootSpawnPoints.Length; i++)
        {
            possibleLootSpawns.Add(lootSpawnPoints[i]);
        }

        InvokeRepeating("SpawnItems", spawnTime, spawnTime);
    }

    private void Update()
    {
        if(GameControl.gameControl.playerCurrentHealth <= 0)
        {
            CancelInvoke();
        }
    }

    private void SpawnItems()
    {
        if(possibleLootSpawns.Count > 0)
        {
            int spawnPointIndex = Random.Range(0, possibleLootSpawns.Count);
            int spawnLootObject = Random.Range(0, lootToSpawn.Length);

            GameObject newLootObject = Instantiate(lootToSpawn[spawnLootObject],
                possibleLootSpawns[spawnPointIndex].position, Quaternion.identity) as GameObject;
            newLootObject.GetComponent<WeaponDrops>().lootSpawnPoint = possibleLootSpawns[spawnPointIndex];

            possibleLootSpawns.RemoveAt(spawnPointIndex);
        }
    }
}
