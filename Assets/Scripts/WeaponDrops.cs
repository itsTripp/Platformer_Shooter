using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrops : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _weaponDrop;
    private Weapon _weaponScript;
    [SerializeField]
    private GameObject _weaponContainer;

    public int[] _weaponTable =
    {
        50,
        50
    };
    private int _weaponTotalWeight;
    private int _weaponRandomNumber;

    // Start is called before the first frame update
    void Start()
    {
        
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
                GameObject newWeapon = Instantiate(_weaponDrop[i], transform.position, 
                    Quaternion.identity);
                newWeapon.transform.parent = _weaponContainer.transform;
                _weaponScript = newWeapon.GetComponent<Weapon>();
                _weaponScript.enabled = false;
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

            ChooseWeapon();
            Destroy(gameObject);
        }
    }
}
