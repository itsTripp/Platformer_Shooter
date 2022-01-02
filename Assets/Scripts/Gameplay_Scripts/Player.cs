using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace EpicTortoiseStudios
{
    public class Player : MonoBehaviour
    {
        private static Player playerInstance;

        [Header("Player Movement")]
        [SerializeField]
        private float _speed;
        private Vector3 _jump;
        [SerializeField]
        private float _jumpForce = 2.0f;
        [SerializeField]
        private bool _isGrounded;
        private bool _isPlayerRunning = false;

        [Header("Weapon Info")]
        private float canShootRightWeapon = 0;
        private float canShootLeftWeapon = 0;
        public GameObject _rightWeapon;
        public GameObject _leftWeapon;

        [Header("Player Score")]
        [SerializeField]
        private int _score;
        [SerializeField]
        private int _experienceOnPickup = 5;

        [Header("Player Audio")]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _jumpAudio;
        [SerializeField]
        private AudioClip _damageTakenAudio;
        [SerializeField]
        private AudioClip _deathAudio;

        private Rigidbody2D _rigidbody;
        private UIManager _uiManager;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        public LootPickups lootPickups;
        private EnemySpawnManager _enemySpawnManager;
        public GameObject PopupText;

        // Start is called before the first frame update
        void Start()
        {
            GameControl.gameControl.playerCurrentHealth = GameControl.gameControl.playerMaxHealth;
            _rigidbody = GetComponent<Rigidbody2D>();
            _jump = new Vector3(0, 2.0f, 0);
            _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
            if (_uiManager == null)
            {
                Debug.LogError("UI Manager is Null on the Player");
            }
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("The Audio Source on the Player is Null");
            }
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator is Null on the Player.");
            }
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("Sprite Renderer is Null on the Player");
            }
            if (GameControl.gameControl.currentLeftWeapon != null)
            {
                _leftWeapon.GetComponent<SpriteRenderer>().sprite =
                    GameControl.gameControl.currentLeftWeapon.currentWeaponSprite;

            }
            if (GameControl.gameControl.currentRightWeapon != null)
            {
                _rightWeapon.GetComponent<SpriteRenderer>().sprite =
                    GameControl.gameControl.currentRightWeapon.currentWeaponSprite;

            }
            _enemySpawnManager = GameObject.Find("Enemy_Spawn_Manager").GetComponent<EnemySpawnManager>();
            if (_enemySpawnManager == null)
            {
                Debug.LogError("Enemy Spawn Manager is Null on the Player");
            }
            DontDestroyOnLoad(this);
            if(playerInstance == null)
            {
                playerInstance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            CalculateMovement();
            _uiManager.UpdateAmmoCount();
            _uiManager.UpdateWeapon();
            if (Input.GetMouseButtonDown(1))
            {
                if (Time.time >= canShootRightWeapon)
                {
                    if (GameControl.gameControl.rightWeaponCurrentAmmo > 0)
                    {
                        GameControl.gameControl.currentRightWeapon.ShootRightWeapon();
                        canShootRightWeapon = Time.time + 1 /
                            GameControl.gameControl.currentRightWeapon.fireRate;
                        GameControl.gameControl.rightWeaponCurrentAmmo--;
                        _uiManager.UpdateAmmoCount();
                        Debug.Log("ShootRight");
                    }

                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= canShootLeftWeapon)
                {
                    if (GameControl.gameControl.leftWeaponCurrentAmmo > 0)
                    {
                        GameControl.gameControl.currentLeftWeapon.ShootLeftWeapon();
                        canShootLeftWeapon = Time.time + 1 /
                            GameControl.gameControl.currentLeftWeapon.fireRate;
                        GameControl.gameControl.leftWeaponCurrentAmmo--;
                        _uiManager.UpdateAmmoCount();
                        Debug.Log("ShootLeft");
                    }

                }
            }

        }

        private void CalculateMovement()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rigidbody.AddForce(_jump * _jumpForce, ForceMode2D.Impulse);
                _isGrounded = false;
                _audioSource.PlayOneShot(_jumpAudio);
            }

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            Vector3 direction = new Vector3(horizontalInput, 0, 0);
            transform.Translate(direction * _speed * Time.deltaTime);

            if (horizontalInput < 0)
            {
                _spriteRenderer.flipX = true;
            }
            if (horizontalInput > 0)
            {
                _spriteRenderer.flipX = false;
            }

            if (horizontalInput != 0)
            {
                _isPlayerRunning = true;
            }
            else
            {
                _isPlayerRunning = false;
            }

            if (_isPlayerRunning == true)
            {
                _animator.SetBool("_isRunning", true);
            }
            else
            {
                _animator.SetBool("_isRunning", false);
            }
            //Screen Wrapping
            if (transform.position.x > 11f)
            {
                transform.position = new Vector3(-11f, transform.position.y, 0);
            }
            if (transform.position.x < -11f)
            {
                transform.position = new Vector3(11f, transform.position.y, 0);
            }
            if (transform.position.y < -1.5f)
            {
                transform.position = new Vector3(transform.position.x, 11f, 0);
            }
            if (transform.position.y > 11f)
            {
                transform.position = new Vector3(transform.position.x, -1.5f, 0);
            }
        }

        public void AddScore(int points)
        {
            _score += points;
            _uiManager.UpdateScore(_score);
        }

        private void OnCollisionStay2D()
        {

            if (!_isGrounded && _rigidbody.velocity.y == 0)
            {
                _isGrounded = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.tag == "Enemy")
            {
                GameControl.gameControl.Damage(1);
                _animator.SetTrigger("_onPlayerHit");
                _uiManager.SetHealth(GameControl.gameControl.GetHealthNormalized());
                _audioSource.PlayOneShot(_damageTakenAudio);
                if (GameControl.gameControl.playerCurrentHealth <= 0)
                {
                    _audioSource.PlayOneShot(_deathAudio);
                    Destroy(gameObject, .5f);
                    _enemySpawnManager.OnPlayerDeath();
                }
            }
            if (other.gameObject.tag == "Floor" && _isGrounded == false)
            {
                _isGrounded = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.tag == "Experience_Pickup")
            {
                _uiManager.AddExperience(_experienceOnPickup);
                //lootPickups = other.GetComponent<PickupItem>().lootPickup;
                Destroy(other.gameObject);
            }
            if (other.tag == "Pistol_Ammo_Pickup")
            {
                if (GameControl.gameControl.currentLeftWeapon != null)
                {
                    if (GameControl.gameControl.currentLeftWeapon.weaponType == Weapon.WeaponType.Pistol)
                    {
                        GameControl.gameControl.leftWeaponCurrentAmmo = GameControl.gameControl.leftWeaponCurrentAmmo + lootPickups.ammoPickupAmount;
                        _uiManager.UpdateAmmoCount();
                        Destroy(other.gameObject);
                    }
                }
                if (GameControl.gameControl.currentRightWeapon != null)
                {
                    if (GameControl.gameControl.currentRightWeapon.weaponType == Weapon.WeaponType.Pistol)
                    {
                        GameControl.gameControl.rightWeaponCurrentAmmo = GameControl.gameControl.rightWeaponCurrentAmmo + lootPickups.ammoPickupAmount;
                        _uiManager.UpdateAmmoCount();
                        Destroy(other.gameObject);
                    }
                }
                //lootPickups = other.GetComponent<PickupItem>().lootPickup;
                //var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                //go.GetComponent<TextMeshPro>().text = lootPickups.popupText;
                _uiManager.UpdateAmmoCount();
                Destroy(other.gameObject);
            }
            if (other.tag == "Shotgun_Ammo_Pickup")
            {
                if (GameControl.gameControl.currentLeftWeapon != null)
                {
                    if (GameControl.gameControl.currentLeftWeapon.weaponType == Weapon.WeaponType.Shotgun)
                    {
                        GameControl.gameControl.leftWeaponCurrentAmmo = GameControl.gameControl.leftWeaponCurrentAmmo + lootPickups.ammoPickupAmount;
                        _uiManager.UpdateAmmoCount();
                        Destroy(other.gameObject);
                    }
                }
                if (GameControl.gameControl.currentRightWeapon != null)
                {
                    if (GameControl.gameControl.currentRightWeapon.weaponType == Weapon.WeaponType.Shotgun)
                    {
                        GameControl.gameControl.rightWeaponCurrentAmmo = GameControl.gameControl.rightWeaponCurrentAmmo + lootPickups.ammoPickupAmount;
                        _uiManager.UpdateAmmoCount();
                        Destroy(other.gameObject);
                    }
                }
                //lootPickups = other.GetComponent<PickupItem>().lootPickup;
                //var go = Instantiate(PopupText, transform.position, Quaternion.identity);
                //go.GetComponent<TextMeshPro>().text = lootPickups.popupText;
                _uiManager.UpdateAmmoCount();
                Destroy(other.gameObject);
            }
        }
    }
}
