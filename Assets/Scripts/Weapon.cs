using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _firePoint;
    [SerializeField]
    private Projectile _projectile;
    private bool _playerCanShoot;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
        
    public int _currentAmmo;   
    public int _maximumAmmo;

    private Player _player;
    private UIManager _uiManager;


    

    // Start is called before the first frame update
    void Start()
    {
         _maximumAmmo = _currentAmmo;
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null on Weapon");
        }
        _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Mananger is Null on PlayerPickups");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShootRightWeapon()
    { 
        if(_currentAmmo > 0)
        {
            _playerCanShoot = true;
            _canFire = Time.time + _fireRate;
            
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            //_projectile.Shoot();
            _currentAmmo--;
            _uiManager.UpdateAmmoCount();
        }
        else
        {
            _playerCanShoot = false;
        }
        
    }
    public void ShootLeftWeapon()
    {
        if(_currentAmmo > 0)
        {
            _playerCanShoot = true;
            _canFire = Time.time + _fireRate;

            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            //_projectile.ShootLeft();
            _currentAmmo--;
            _uiManager.UpdateAmmoCount();
        }
        else
        {
            _playerCanShoot = false;
        }
        
    }
}
