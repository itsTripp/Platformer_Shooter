using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [Header("Ground Movement")]
    [SerializeField] private float _maxSpeed; //Max speed while on the ground
    [SerializeField] private float _acceleration; //Acceleration while on the ground
    [SerializeField] private float _deAcceleration; //Acceleration while on the ground
    [Header("Air Movement")]
    [SerializeField] private float _airMaxSpeed; //Max speed while in the air
    [SerializeField] private float _airAcceleration; //Acceleration while in the air
    [SerializeField] private float _airDeAcceleration; //Acceleration while in the air
    [SerializeField] private float _maxFallSpeed;
    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _jumpCount; //Times the character can jump before touching the ground.

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Transform _characterBase;
    private CapsuleCollider2D _collider;

    private bool _isGrounded = false;

    private float _currentSpeed = 0f;
    private int _currentJumpCount = 0;

    //Input
    private float _inputX = 0;
    private bool _jump = false;

    //Perk Modifiers
    private float _perkMaxSpeed = 0;
    private float _perkAcceleration = 0;
    private float _perkDeAcceleration = 0;
    private float _perkAirMaxSpeed = 0;
    private float _perkAirAcceleratrion = 0;
    private float _perkAirDeAcceleratrion = 0;
    private float _perkJumpForce = 0;
    private int _perkJumpCount = 0;

    [SerializeField] UnityEvent m_Jump;
    [SerializeField] UnityEvent m_DoubleJump;
    [SerializeField] UnityEvent m_Land;


    // Start is called before the first frame update
    void Start()
    {
        string missingComponents = "";
        if (!TryGetComponent(out _collider))
        {
            missingComponents += "CapsuleCollider2D,";
        }
        if (!TryGetComponent(out _rigidbody2D))
        {
            missingComponents += "RigidBody2D,";
        }
        _spriteRenderer = this.transform.GetComponentInChildren<SpriteRenderer>(true);
        if (_spriteRenderer == null)
        {
            missingComponents += "SpriteRenderer,";
        }
        _animator = this.transform.GetComponentInChildren<Animator>(true);
        if (_animator == null)
        {
            missingComponents += "Animator,";
        }
        _characterBase = this.transform.Find("CharacterBase");
        if (_characterBase == null)
        {
            Debug.LogError(this.gameObject.name + " is missing required child 'CharacterBase'. This child is used to determine if character is grounded and should be placed at the 'feet' of the character.");
        }

        if (missingComponents.Length > 0)
        {
            missingComponents = missingComponents.Trim(','); //Trim the last comma off.
            Debug.LogError(this.gameObject.name + " is missing the follow required components for its 'Movement' component (" + missingComponents + ")");
        }
    }

    private void FixedUpdate()
    {
        //Check if Grounded (Use Raycast to check)
        CheckGrounded();

        //Handle Jumping
        Jump();

        //Move the player on the x Axis
        Move();
    }

    private void CheckGrounded()
    {
        int layer_mask = LayerMask.GetMask("World");
        RaycastHit2D hit = Physics2D.Raycast(_characterBase.position, -Vector2.up, .1f, layer_mask);
        
        if (hit.transform != null)
        {
            Debug.Log(hit.transform.name.ToString());
            if (_isGrounded == false) m_Land.Invoke();
            _isGrounded = true;
            _currentJumpCount = 0;

            if (_animator)
            {
                _animator.SetBool("_isGrounded", true);
            }
        } else
        {
            if (_animator)
            {
                _animator.SetBool("_isGrounded", false);
                _isGrounded = false;
            }
        }
    }

    public void Jump()
    {
        //Can we jump?
        if (_jump && _currentJumpCount < (_jumpCount + _perkJumpCount))
        {
            _jump = false;
            Debug.Log("Jump Force: " + _jumpForce.ToString());
            _rigidbody2D.AddForce(Vector2.up * (_jumpForce + _perkJumpForce), ForceMode2D.Impulse);
            _currentJumpCount++;

            m_Jump.Invoke();
        }
        else
        {
            _jump = false;
        }
    }

    public void Move()
    {
        if (_inputX != 0)
        {
            if (_isGrounded) _currentSpeed = _inputX * Mathf.Clamp(Mathf.Abs(_currentSpeed) + (_acceleration + _perkAcceleration), 0, (_maxSpeed + _perkMaxSpeed));
            if (!_isGrounded) _currentSpeed = _inputX * Mathf.Clamp(Mathf.Abs(_currentSpeed) + (_airAcceleration + _perkAirAcceleratrion), 0, (_airMaxSpeed + _perkAirMaxSpeed));
        } else
        {
            if (_isGrounded) _currentSpeed = Mathf.Sign(_currentSpeed) * Mathf.Clamp(Mathf.Abs(_currentSpeed) - (_deAcceleration + _perkDeAcceleration), 0, _currentSpeed);
            if (!_isGrounded) _currentSpeed = Mathf.Sign(_currentSpeed) * Mathf.Clamp(Mathf.Abs(_currentSpeed) - (_airDeAcceleration + _perkAirDeAcceleratrion), 0, _currentSpeed);
        }

        //Check if there is a wall blocking left or right
        

        _rigidbody2D.velocity = new Vector2(_currentSpeed, Mathf.Clamp(_rigidbody2D.velocity.y, -_maxFallSpeed, (_jumpForce + _perkJumpForce)));

        float xVelocity = _rigidbody2D.velocity.x;
        if (xVelocity < 0)
        {
            //Character is moving right
            if (_spriteRenderer) _spriteRenderer.flipX = true;
            if (_animator) _animator.SetBool("_isRunning", true);
        } else if (xVelocity > 0)
        {
            //Character is moving left
            if (_spriteRenderer) _spriteRenderer.flipX = false;
            if (_animator) _animator.SetBool("_isRunning", true);
        } else
        {
            //Character is stopped
            if (_animator) _animator.SetBool("_isRunning", false);
        }
    }

    public void OnDisable()
    {
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _inputX = context.ReadValue<Vector2>().x;
        if (_animator) _animator.SetFloat("_inputX", _inputX);
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) _jump = true;
    }
}
