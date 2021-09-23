using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    //private Weapon _weapon;
    public WeaponEquip _weaponEquip;
    public static WeaponEquip instance;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private Transform _rightGunContainer;
    [SerializeField]
    private Transform _leftGunContainer;
    private Weapon _leftWeapon;
    private Weapon _rightWeapon;

    [SerializeField]
    private float _pickupRange;
    [SerializeField]
    private float _dropForwardForce;
    [SerializeField]
    private float _dropUpwardForce;

    public bool _rightGunIsEquipped;
    public bool _leftGunIsEquipped;
    public static bool rightGunSlotIsFull;
    public static bool leftGunSlotIsFull;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _rightGunContainer = GameObject.Find("Right_Weapon").GetComponent<Transform>();
        _leftGunContainer = GameObject.Find("Left_Weapon").GetComponent<Transform>();
    }

    void Start()
    {
        Object[] allWeapons = Object.FindObjectsOfType<Weapon>();
        _leftWeapon = (Weapon)allWeapons[0];
        _rightWeapon = (Weapon)allWeapons[1];
        /*_weapon = GetComponent<Weapon>();
        if (_weapon == null)
        {
            Debug.LogError("Weapon is Null in WeaponEquip");
        }*/
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody2D is Null in WeaponEquip");
        }
        _boxCollider = GetComponent<BoxCollider2D>();
        if (_boxCollider == null)
        {
            Debug.LogError("BoxCollider2D is Null in WeaponEquip");
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        

        //Right Weapon
        if (!_rightGunIsEquipped)
        {
            _rightWeapon.enabled = false;
            _rigidbody.isKinematic = false;
            _boxCollider.isTrigger = false;
        }
        if (_rightGunIsEquipped)
        {
            _rightWeapon.enabled = true;
            _rigidbody.isKinematic = true;
            _boxCollider.isTrigger = true;
            rightGunSlotIsFull = true;
        }
        //Left Weapon
        if (!_leftGunIsEquipped)
        {
            
            _leftWeapon.enabled = false;
            _rigidbody.isKinematic = false;
            _boxCollider.isTrigger = false;
        }
        if (_leftGunIsEquipped)
        {
            _spriteRenderer.flipX = true;
            _leftWeapon.enabled = true;
            _rigidbody.isKinematic = true;
            _boxCollider.isTrigger = true;
            leftGunSlotIsFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = _playerTransform.position - transform.position;
        //Right Weapon
        if (!_rightGunIsEquipped && distanceToPlayer.magnitude <= _pickupRange && Input.GetKeyDown(KeyCode.E)
        && !rightGunSlotIsFull)
        {
            PickupRightWeapon();
        }
        if (_rightGunIsEquipped && Input.GetKeyDown(KeyCode.R))
        {
            DropRightWeapon();
        }
        //Left Weapon
        if (!_leftGunIsEquipped && distanceToPlayer.magnitude <= _pickupRange && Input.GetKeyDown(KeyCode.Q)
        && !leftGunSlotIsFull)
        {
            PickupLeftWeapon();
        }
        if (_leftGunIsEquipped && Input.GetKeyDown(KeyCode.F))
        {
            DropLeftWeapon();
        }
    }

    public void PickupWeapon(int id)
    {
        if(_leftWeapon.GetInstanceID() == id)
        {
            this.PickupLeftWeapon();
        }
        else if (_rightWeapon.GetInstanceID() == id)
        {
            this.PickupRightWeapon();
        }
    }
    public void PickupRightWeapon()
    {
        _rightGunIsEquipped = true;
        rightGunSlotIsFull = true;

        transform.SetParent(_rightGunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        _rigidbody.isKinematic = true;
        _boxCollider.isTrigger = true;

        _rightWeapon.enabled = true;
    }

    private void DropRightWeapon()
    {
        _rightGunIsEquipped = false;
        rightGunSlotIsFull = false;

        transform.SetParent(null);

        _rigidbody.isKinematic = false;
        _boxCollider.isTrigger = false;

        _rigidbody.velocity = _playerTransform.GetComponent<Rigidbody2D>().velocity;

        _rigidbody.AddForce(_playerTransform.forward * _dropForwardForce, ForceMode2D.Impulse);
        _rigidbody.AddForce(_playerTransform.up * _dropUpwardForce, ForceMode2D.Impulse);

        _rightWeapon.enabled = false;
    }

    public void PickupLeftWeapon()
    {
        _leftGunIsEquipped = true;
        leftGunSlotIsFull = true;

        transform.SetParent(_leftGunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        _rigidbody.isKinematic = true;
        _boxCollider.isTrigger = true;

        _leftWeapon.enabled = true;
    }

    private void DropLeftWeapon()
    {
        _leftGunIsEquipped = false;
        leftGunSlotIsFull = false;

        transform.SetParent(null);

        _rigidbody.isKinematic = false;
        _boxCollider.isTrigger = false;

        _rigidbody.velocity = _playerTransform.GetComponent<Rigidbody2D>().velocity;

        _rigidbody.AddForce(_playerTransform.forward * _dropForwardForce, ForceMode2D.Impulse);
        _rigidbody.AddForce(_playerTransform.up * _dropUpwardForce, ForceMode2D.Impulse);

        _leftWeapon.enabled = false;
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Right_Weapon")
        {
            if(!_rightGunIsEquipped && !rightGunSlotIsFull || _leftGunIsEquipped)
            {
                    PickupRightWeapon();
                    Debug.Log("Right");
            }
            if(!_leftGunIsEquipped && !leftGunSlotIsFull || _rightGunIsEquipped)
            {
                    PickupLeftWeapon();
                    Debug.Log("Left");
            }
        }

        if (other.gameObject.tag == "Left_Weapon")
        {
            if (!_leftGunIsEquipped && !leftGunSlotIsFull || _rightGunIsEquipped)
            {
                PickupLeftWeapon();
                Debug.Log("Left");
            }
            if(!_leftGunIsEquipped && !leftGunSlotIsFull || _rightGunIsEquipped)
            {
                    PickupLeftWeapon();
                    Debug.Log("Left");
            }
        }
    }*/
}
