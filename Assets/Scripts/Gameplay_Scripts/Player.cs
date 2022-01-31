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

        [Header("Weapons")]
        [SerializeField]
        private Weapon rightWeapon;
        [SerializeField]
        private Weapon leftWeapon;

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
        private BoxCollider2D _boxcollider2d;
        private UIManager _uiManager;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        public LootPickups lootPickups;
        private EnemySpawnManager _enemySpawnManager;
        public GameObject PopupText;

        private Transform rightWeaponEquip;
        private Transform leftWeaponEquip;
        private Inventory inventory;
        private Interactor interactor;

        // Start is called before the first frame update
        void Start()
        {
            rightWeaponEquip = this.transform.Find("Gun_Container").Find("Right_Weapon");
            leftWeaponEquip = this.transform.Find("Gun_Container").Find("Left_Weapon");
            inventory = this.GetComponent<Inventory>();
            interactor = this.transform.Find("Interactor").GetComponent<Interactor>();

            GameControl.gameControl.playerCurrentHealth = GameControl.gameControl.playerMaxHealth;
            _rigidbody = GetComponent<Rigidbody2D>();
            _jump = new Vector3(0, 2.0f, 0);
            _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
            if (_uiManager == null)
            {
                Debug.LogError("UI Manager is Null on the Character");
            }
            _audioSource = this.transform.Find("Character").GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("The Audio Source on the Character is Null");
            }
            _animator = this.transform.Find("Character").GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator is Null on the Character.");
            }
            _spriteRenderer = this.transform.Find("Character").GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("Sprite Renderer is Null on the Character");
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
            if(controller.Player_Movement.Jump.triggered)
            {
                OnJump();
            }

            CalculateMovement();
            _uiManager.UpdateAmmoCount();
            _uiManager.UpdateWeapon();
            if (Input.GetMouseButton(1))
            {
                if (rightWeapon)
                {
                    rightWeapon.Fire();
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (leftWeapon)
                {
                    leftWeapon.Fire();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (leftWeapon)
                {
                    leftWeapon.UnequipFromPlayer();
                    leftWeapon = null;
                }
                
                if (interactor.equipableWeapon != null)
                {
                    leftWeapon = interactor.equipableWeapon;
                    interactor.equipableWeapon = null;
                    leftWeapon.EquipToPlayer(leftWeaponEquip, inventory, this);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (rightWeapon)
                {
                    rightWeapon.UnequipFromPlayer();
                    rightWeapon = null;
                }
                
                if (interactor.equipableWeapon != null)
                {
                    rightWeapon = interactor.equipableWeapon;
                    interactor.equipableWeapon = null;
                    rightWeapon.EquipToPlayer(rightWeaponEquip, inventory, this);
                }
            }
        }

        void OnJump()
        {
            _rigidbody.AddForce(_jump * _jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
            _audioSource.PlayOneShot(_jumpAudio);
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

        public void ApplyKnock(Vector3 direction, float intensity)
        {
            Vector2 convertedDirection = new Vector2(direction.x, direction.y);
            _rigidbody.AddForce(direction * intensity);
        }
    }
}
