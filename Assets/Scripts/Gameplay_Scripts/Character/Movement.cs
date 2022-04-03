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
    [SerializeField] private float _jumpAirForcePerc = .5f;
    [SerializeField] private int _jumpCount; //Times the character can jump before touching the ground.
    [SerializeField] private float _coyoteTime = .4f;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer; //Body Sprite Renderer
    private SpriteRenderer _rightArmSpriteRenderer; //Right Arm Sprite Renderer
    private SpriteRenderer _leftArmSpriteRenderer; //Left Arm Sprite Renderer
    private Animator _animator;
    private Transform _characterBase;
    private CapsuleCollider2D _collider;

    private bool _isGrounded = false;

    private float _currentSpeed = 0f;
    private int _currentJumpCount = 0;
    private float _coyoteTimer = 0f;

    //Input
    private float _inputX = 0;
    private bool _jumpHeld = false;
    private bool _jumpStarted = false;

    //Perk Modifiers
    private float _perkMaxSpeed = 0;
    private float _perkAcceleration = 0;
    private float _perkDeAcceleration = 0;
    private float _perkAirMaxSpeed = 0;
    private float _perkAirAcceleratrion = 0;
    private float _perkAirDeAcceleratrion = 0;
    private float _perkJumpForce = 0;
    private int _perkJumpCount = 0;

    [Header("Events")]
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
            missingComponents += "Body_SpriteRenderer,";
        }
        _rightArmSpriteRenderer = GameObject.Find("Right_Arm").GetComponent<SpriteRenderer>();
        if (_rightArmSpriteRenderer == null)
        {
            missingComponents += "Front_Arm_SpriteRenderer,";
        }
        _leftArmSpriteRenderer = GameObject.Find("Left_Arm").GetComponent<SpriteRenderer>();
        if (_leftArmSpriteRenderer == null)
        {
            missingComponents += "Rear_Arm_SpriteRenderer,";
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

        _animator.SetFloat("_yVelocity", _rigidbody2D.velocity.y);
        _animator.SetFloat("_xVelocity", _rigidbody2D.velocity.x);
    }

    private void CheckGrounded()
    {
        int layer_mask = LayerMask.GetMask("World/AllCollision");
        float halfWidth = _collider.bounds.size.x / 2;
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(_characterBase.position.x - halfWidth, _characterBase.position.y), -Vector2.up, .1f, layer_mask);
        RaycastHit2D hitMid = Physics2D.Raycast(_characterBase.position, -Vector2.up, .1f, layer_mask);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(_characterBase.position.x + halfWidth, _characterBase.position.y), -Vector2.up, .1f, layer_mask);

        if (hitLeft.transform != null || hitMid.transform != null || hitRight.transform != null)
        {
            if (_isGrounded == false) m_Land.Invoke();
            _isGrounded = true;
            _currentJumpCount = 0;
            _coyoteTimer = 0;

            if (_animator)
            {
                _animator.SetBool("_isGrounded", true);
            }
        } else
        {
            _coyoteTimer += Time.deltaTime;

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
        if ((_jumpHeld && !_jumpStarted) && _currentJumpCount < (_jumpCount + _perkJumpCount))
        {
            if (_currentJumpCount == 0)
            {
                //Need to determine if the player is grounded or just left the ground. CoyoteTime.
                if (_coyoteTimer <= _coyoteTime)
                {
                    _jumpStarted = true;
                    if (_rigidbody2D.velocity.y < 0) _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                    _rigidbody2D.AddForce(Vector2.up * (_jumpForce + _perkJumpForce), ForceMode2D.Impulse);
                    _currentJumpCount++;

                    m_Jump.Invoke();
                }

            } else
            {
                //In Air Jump
                if (_rigidbody2D.velocity.y < 0) _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.AddForce(Vector2.up * ((_jumpForce + _perkJumpForce) * _jumpAirForcePerc), ForceMode2D.Impulse);
                _currentJumpCount++;

                m_Jump.Invoke();
            }
        }

        if (!_jumpHeld && _jumpStarted && _rigidbody2D.velocity.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2);
            _jumpStarted = false;
        }
    }

    public void Move()
    {
        float frameAcceleration = (_acceleration + _perkAcceleration) * Time.deltaTime;
        float frameAirAcceleration = (_airAcceleration + _perkAirAcceleratrion) * Time.deltaTime;
        float frameDeaccelration = (_deAcceleration + _perkDeAcceleration) * Time.deltaTime;
        float frameAirDeacceleration = (_airDeAcceleration + _perkAirDeAcceleratrion) * Time.deltaTime;

        if (!canMoveSide((int)Mathf.Sign(_inputX))) _inputX = 0;

        if (_inputX != 0)
        {
            if (_isGrounded) _currentSpeed = _inputX * Mathf.Clamp(Mathf.Abs(_currentSpeed) + frameAcceleration, 0, (_maxSpeed + _perkMaxSpeed));
            if (!_isGrounded) _currentSpeed = _inputX * Mathf.Clamp(Mathf.Abs(_currentSpeed) + frameAirAcceleration, 0, (_airMaxSpeed + _perkAirMaxSpeed));
        } else
        {
            if (_isGrounded) _currentSpeed = Mathf.Sign(_currentSpeed) * Mathf.Clamp(Mathf.Abs(_currentSpeed) - frameDeaccelration, 0, Mathf.Abs(_currentSpeed));
            if (!_isGrounded) _currentSpeed = Mathf.Sign(_currentSpeed) * Mathf.Clamp(Mathf.Abs(_currentSpeed) - frameAirDeacceleration, 0, Mathf.Abs(_currentSpeed));
        }

        _rigidbody2D.velocity = new Vector2(_currentSpeed, Mathf.Clamp(_rigidbody2D.velocity.y, -_maxFallSpeed, (_jumpForce + _perkJumpForce)));

        float xVelocity = _rigidbody2D.velocity.x;
        if (xVelocity < 0)
        {
            //Character is moving right
            if (_spriteRenderer) _spriteRenderer.flipX = true;
            if (_rightArmSpriteRenderer) _rightArmSpriteRenderer.flipX = true;
            if (_leftArmSpriteRenderer) _leftArmSpriteRenderer.flipX = true;
            if (_animator) _animator.SetBool("_isRunning", true);
        } else if (xVelocity > 0)
        {
            //Character is moving left
            if (_spriteRenderer) _spriteRenderer.flipX = false;
            if (_rightArmSpriteRenderer) _rightArmSpriteRenderer.flipX = false;
            if (_leftArmSpriteRenderer) _leftArmSpriteRenderer.flipX = false;
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
        if (context.performed) _jumpHeld = true;
        if (context.canceled) _jumpHeld = false;
    }

    private bool canMoveSide(int dir)
    {
        int layer_mask = LayerMask.GetMask("World");
        float halfWidth = (_collider.bounds.size.x / 2) * dir;
        float halfHeight = (_collider.bounds.size.y / 2);
        RaycastHit2D hitHigh = Physics2D.Raycast(new Vector2(_characterBase.position.x + halfWidth, _characterBase.position.y + (halfHeight * 2)), Vector2.right * dir, .1f, layer_mask);
        RaycastHit2D hitMid = Physics2D.Raycast(new Vector2(_characterBase.position.x + halfWidth, _characterBase.position.y + halfHeight), Vector2.right * dir, .1f, layer_mask);
        //RaycastHit2D hitLow = Physics2D.Raycast(new Vector2(_characterBase.position.x + halfWidth, _characterBase.position.y), Vector2.right * dir, .1f, layer_mask);

        if (hitHigh.transform != null || hitMid.transform != null)
        {
            return false;
        }

        return true;
    }
}
