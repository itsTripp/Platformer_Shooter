using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace EpicTortoiseStudios
{
    public class Player : MonoBehaviour
    {

        public PlayerController controller;

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
        private float inputX;

        [Header("Weapon Info")]
        private float canShootRightWeapon = 0;
        private float canShootLeftWeapon = 0;
        public GameObject _rightWeapon;
        public GameObject _leftWeapon;

        [Header("Player Score")]
        [SerializeField]
        private int _score;
        //[SerializeField]
        public int _experienceOnPickup = 5;

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

        private void Awake()
        {
            controller = new PlayerController();
        }

        // Update is called once per frame
        void Update()
        {
            _rigidbody.velocity = new Vector2(inputX * _speed, _rigidbody.velocity.y);
            ScreenWrapping();
            _uiManager.UpdateAmmoCount();
            _uiManager.UpdateWeapon();
        }

        private void OnEnable()
        {
            controller.Player_Movement.Enable();
        }

        private void OnDisable()
        {
            controller.Player_Movement.Disable();
        }

        public void Move(InputAction.CallbackContext context)
        {
            inputX = context.ReadValue<Vector2>().x;

            if (inputX < 0)
            {
                _spriteRenderer.flipX = true;
            }
            if (inputX > 0)
            {
                _spriteRenderer.flipX = false;
            }

            if (inputX != 0)
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
        }

        private void ScreenWrapping()
        {
            if (transform.position.x > 10.5f)
            {
                transform.position = new Vector3(-10.5f, transform.position.y, 0);
            }
            if (transform.position.x < -10.5f)
            {
                transform.position = new Vector3(10.5f, transform.position.y, 0);
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

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrounded == true)
            {
                _rigidbody.AddForce(_jump * _jumpForce, ForceMode2D.Impulse);
                _isGrounded = false;
                _audioSource.PlayOneShot(_jumpAudio);
            }
        }

        public void Shoot_Left(InputAction.CallbackContext context)
        {
            if (Time.time >= canShootLeftWeapon)
            {
                if (GameControl.gameControl.currentLeftWeapon.weaponType == Weapon.WeaponType.Pistol)
                {
                    if (GameControl.gameControl.pistolAmmo > 0)
                    {
                        GameControl.gameControl.currentLeftWeapon.ShootLeftWeapon();
                        canShootLeftWeapon = Time.time + 1 /
                            GameControl.gameControl.currentLeftWeapon.fireRate;
                        GameControl.gameControl.pistolAmmo--;
                        _uiManager.UpdateAmmoCount();
                        Debug.Log("ShootLeft");
                    }
                }
                if (GameControl.gameControl.currentLeftWeapon.weaponType == Weapon.WeaponType.Shotgun)
                {
                    if (GameControl.gameControl.shotgunAmmo > 0)
                    {
                        GameControl.gameControl.currentLeftWeapon.ShootLeftWeapon();
                        canShootLeftWeapon = Time.time + 1 /
                            GameControl.gameControl.currentLeftWeapon.fireRate;
                        GameControl.gameControl.shotgunAmmo--;
                        _uiManager.UpdateAmmoCount();
                        Debug.Log("ShootLeft");
                    }
                }
            }
        }

        public void Shoot_Right(InputAction.CallbackContext context)
        {
            if (Time.time >= canShootRightWeapon)
            {
                if (GameControl.gameControl.currentRightWeapon.weaponType == Weapon.WeaponType.Pistol)
                {
                    if (GameControl.gameControl.pistolAmmo > 0)
                    {
                        GameControl.gameControl.currentRightWeapon.ShootRightWeapon();
                        canShootRightWeapon = Time.time + 1 /
                            GameControl.gameControl.currentRightWeapon.fireRate;
                        GameControl.gameControl.pistolAmmo--;
                        _uiManager.UpdateAmmoCount();
                        Debug.Log("ShootRight");
                    }
                }
                if (GameControl.gameControl.currentRightWeapon.weaponType == Weapon.WeaponType.Shotgun)
                {
                    if (GameControl.gameControl.shotgunAmmo > 0)
                    {
                        GameControl.gameControl.currentRightWeapon.ShootRightWeapon();
                        canShootRightWeapon = Time.time + 1 /
                            GameControl.gameControl.currentRightWeapon.fireRate;
                        GameControl.gameControl.shotgunAmmo--;
                        _uiManager.UpdateAmmoCount();
                        Debug.Log("ShootRight");
                    }
                }

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
    }
}
