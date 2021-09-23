using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField]
    private int _playerMaxHealth = 5;
    [SerializeField]
    private int _playerCurrentHealth;

    [Header("Player Movement")]
    [SerializeField]
    private float _speed;

    [Header("Weapon Info")]
  
    public GameObject _rightWeapon;
    [SerializeField]
    private GameObject _activeRightWeapon;
    
    public GameObject _leftWeapon;
    [SerializeField]
    private GameObject _activeLeftWeapon;
    private bool _isRightWeaponActive;
    private bool _isLeftWeaponActive;

    private Vector3 _jump;
    [SerializeField]
    private float _jumpForce = 2.0f;
    [SerializeField]
    private bool _isGrounded;
    private bool _isPlayerRunning = false;

    [Header("Player Score")]
    [SerializeField]
    private int _score;

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
    private Weapon _weapon;
    public WeaponEquip _weaponEquip;
    private WeaponEquip _leftWeaponEquip;
    private WeaponEquip _rightWeaponEquip;

    // Start is called before the first frame update
    void Start()
    {
        //Object[] allWeapons = Object.FindObjectsOfType<WeaponEquip>();
        //_weaponEquip = (WeaponEquip)allWeapons[0];
        _playerCurrentHealth = _playerMaxHealth;
        _rigidbody = GetComponent<Rigidbody2D>();
        _jump = new Vector3(0, 2.0f, 0);
        _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is Null on the Player");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Player is Null");
        }
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is Null on the Player.");
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is Null on the Player");
        }
        /*GameObject[] Weapons = GameObject.FindGameObjectsWithTag("Weapon");
        _weapons = new Weapon[Weapons.Length];
        for (int i = 0; i < Weapons.Length; i++)
        {
            _weapons[i] = Weapons[i].GetComponent<Weapon>();
            _weaponEquip = Weapons[i].GetComponent<WeaponEquip>();
        }*/

        WeaponEquip.instance = _weaponEquip;

        /*try
        {
            _weaponEquip = GameObject.FindGameObjectWithTag("Weapon").GetComponent<WeaponEquip>();
        }
        catch(NullReferenceException f)
        {
            Debug.Log(f);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (WeaponEquip.rightGunSlotIsFull == true && Input.GetButtonDown("Fire2"))
        {
            _weapon.ShootRightWeapon();
            Debug.Log("Shot Right");
        }
        if (WeaponEquip.leftGunSlotIsFull == true && Input.GetButtonDown("Fire1"))
        {
            _weapon.ShootLeftWeapon();
            Debug.Log("Shot Left");
        }
                
    }

    private void CalculateMovement()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidbody.AddForce(_jump * _jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
            _audioSource.PlayOneShot(_jumpAudio);
        }
            
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            Vector3 direction = new Vector3(horizontalInput, 0, 0);
            transform.Translate(direction * _speed * Time.deltaTime);

        if(horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }
        if(horizontalInput > 0)
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

        if(_isPlayerRunning == true)
        {
            _animator.SetBool("_isRunning", true);
        }
        else
        {
            _animator.SetBool("_isRunning", false);
        }
        //Screen Wrapping
        if(transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        if(transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
        if(transform.position.y < -1.5f)
        {
            transform.position = new Vector3(transform.position.x, 11f, 0);
        }
        if(transform.position.y > 11f)
        {
            transform.position = new Vector3(transform.position.x, -1.5f, 0);
        }
    }

    private void OnCollisionStay2D()
    {
        _isGrounded = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Enemy")
        {
            _playerCurrentHealth--;
            _animator.SetTrigger("_onPlayerHit");
            _uiManager.UpdateHealth(_playerCurrentHealth);
            _audioSource.PlayOneShot(_damageTakenAudio);
            if(_playerCurrentHealth <= 0)
            {
                _audioSource.PlayOneShot(_deathAudio);
                Destroy(gameObject,.5f);
            }
        }
        if(other.transform.tag == "Floor")
        {
            _isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            try
            {
                WeaponEquip.instance.PickupWeapon(other.gameObject.GetInstanceID());
            }
            catch(NullReferenceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
