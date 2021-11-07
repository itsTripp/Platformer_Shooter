using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrops : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D[] _weaponDrop;
    private GameObject _weaponContainer;
    [SerializeField]
    private float _upwardForce;
    [SerializeField]
    private float _outwardForce;

    public int[] _weaponTable =
    {
        50,
        50
    };
    [SerializeField]
    private int _weaponTotalWeight;
    private int _weaponRandomNumber;
    [SerializeField]
    private float destroyTime;

    private LootChestSpawner _lootChestSpawner;
    public Transform lootSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        _weaponContainer = GameObject.Find("Weapon_Container");
        if(_weaponContainer == null)
        {
            Debug.LogError("Weapon Container is Null on Weapon Drop Script");
        }
        _lootChestSpawner = GameObject.Find("Loot_Manager").GetComponent<LootChestSpawner>();
        if(_lootChestSpawner == null)
        {
            Debug.LogError("Loot Chest Spawner is Null on Weapon Drop Script");
        }
        foreach (var item in _weaponTable)
        {
            _weaponTotalWeight += item;
        }
        StartCoroutine(DestroyLoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChooseWeapon()
    {
        _weaponRandomNumber = Random.Range(0, _weaponTotalWeight);
        Debug.Log("Weapon Random Number: " + _weaponRandomNumber);
        for (int i = 0; i < _weaponTable.Length; i++)
        {
            if(_weaponRandomNumber <= _weaponTable[i])
            {
                Rigidbody2D newWeapon = Instantiate(_weaponDrop[i], transform.position, 
                    Quaternion.identity) as Rigidbody2D;
                newWeapon.transform.parent = _weaponContainer.transform;
                newWeapon.AddForce(transform.up * _upwardForce);
                newWeapon.AddForce(transform.right * Random.Range(-_outwardForce, _outwardForce));
                return;
            }
            else
            {
                _weaponRandomNumber -= _weaponTable[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < _lootChestSpawner.lootSpawnPoints.Length; i++)
            {
                if (_lootChestSpawner.lootSpawnPoints[i] == lootSpawnPoint)
                {
                    _lootChestSpawner.possibleLootSpawns.Add(_lootChestSpawner.lootSpawnPoints[i]);
                }
            }
            ChooseWeapon();
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyLoot()
    {
        yield return new WaitForSeconds(destroyTime);

        for(int i = 0; i < _lootChestSpawner.lootSpawnPoints.Length; i++)
        {
            if(_lootChestSpawner.lootSpawnPoints[i] == lootSpawnPoint)
            {
                _lootChestSpawner.possibleLootSpawns.Add(_lootChestSpawner.lootSpawnPoints[i]);
            }
        }
        Destroy(gameObject);
    }
}
