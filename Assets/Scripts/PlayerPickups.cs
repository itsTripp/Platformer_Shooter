using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickups : MonoBehaviour
{
    private Player _player;
    private UIManager _uiManager;
    private Weapon _weapons;
    [SerializeField]
    private int _experienceOnPickup = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null on PlayerPickups");
        }
        _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Mananger is Null on PlayerPickups");
        }
        /*GameObject[] Weapons = GameObject.FindGameObjectsWithTag("Weapon");
        _weapons = new Weapon[Weapons.Length];
        for (int i = 0; i < Weapons.Length; i++)
        {
            _weapons[i] = Weapons[i].GetComponent<Weapon>();
        }*/
    }


        // Update is called once per frame
        void Update()
        {

        }


        public void AddAmmo()
        {
            _weapons._currentAmmo = _weapons._maximumAmmo;
            _uiManager.UpdateAmmoCount();
        }

    public void AddExperience()
    {
        
        _uiManager.AddExperience(_experienceOnPickup);
    }
}
