using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBrain : MonoBehaviour
{
    [Header("Command: Jump")] //Settings for handling AICommand Jump.
    [SerializeField] [Tooltip("Will the AI jump when encountering a Jump AICommand box?")] public bool canJump = true;
    [SerializeField] [Tooltip("Height of the AI Jump")] public float jumpForce = 400f;
    [Space(5)]
    [SerializeField] UnityEvent m_Jumped;
    [SerializeField] UnityEvent m_Landed;

    [Space(25)]

    [Header("Command: Turn")]
    [SerializeField] [Tooltip("Will the AI turn when encountering a Turn AICommand box?")] public bool canTurn = true;
    [SerializeField] [Tooltip("Should the AI Idle in place after turning?")] public bool idleAfterTurn = false;
    [Space(5)]
    [SerializeField] UnityEvent m_Turned;

    [Space(25)]

    [Header("State: Idle")] //Will remain at its current position for a random time between the min and max before attempting to patrol.
    [SerializeField] [Tooltip("Can the AI Chase another object?")] public bool canIdle = true;
    [SerializeField] [Tooltip("Minimum time (Seconds) the AI can Idle in place before patroling")] public float idleTimeMin = 1f;
    [SerializeField] [Tooltip("Maximum time (Seconds) the AI can Idle in place before patroling")] public float idleTimeMax = 5f;
    [Space(5)]
    [SerializeField] UnityEvent m_IdleStarted;
    [SerializeField] UnityEvent m_IdleEnded;

    [Space(25)]

    [Header("State: Patrol")] //Will move in one direction until hitting an AICommand Turn, and then patrol in the other direction. Will switch to idle after a random time between the min and max.
    [SerializeField] [Tooltip("Can the AI Chase another object?")] public bool canPatrol = false;
    [SerializeField] [Tooltip("Speed to use on the AI when in Patrol State.")] public float patrolSpeed = 3f;
    [SerializeField] [Tooltip("Should the AI patrol in the other direction at the start of a patrol?")] public bool changeDirection = false;
    [SerializeField] [Tooltip("Minimum time (Seconds) the AI can patrol before idling")] public float patrolTimeMin = 5f;
    [SerializeField] [Tooltip("Maximum time (Seconds) the AI can patrol before idling")] public float patrolTimeMax = 10f;
    [Space(5)]
    [SerializeField] UnityEvent m_PatrolStarted;
    [SerializeField] UnityEvent m_PatrolEnded;

    [Space(25)]

    [Header("State: Chase")] //Will chase the object, moving close to it and pursuing it. If it loses sight of the object it will continue to move toward forward until it finds the object or forgets it.
    [SerializeField] [Tooltip("Can the AI Chase another object?")] public bool canChase = false;
    [SerializeField] [Tooltip("Object Layers that are considered Chase Objects")] public LayerMask chaseMask;
    [SerializeField] [Tooltip("Can the AI see the chase object through walls?")] public bool canSeeThroughWalls = false;
    [SerializeField] [Tooltip("Distance the AI can see the chase objects")] public float sightDistance;
    [SerializeField] [Tooltip("Does the AI only see in the direction it is facing?")] public bool sightFaceDirectionOnly = false;
    [SerializeField] [Tooltip("Speed to use on the AI when in Chase State")] public float chaseSpeed = 4f;
    [SerializeField] [Tooltip("Distance before the AI stops moving towards the chase object")] public float followDistance;
    [SerializeField] [Tooltip("Time (Seconds) before AI returns to idle/patrol state.")] public float timeToForget;
    [Space(5)]
    [SerializeField] UnityEvent m_ChaseStarted;
    [SerializeField] UnityEvent m_ChaseEnded;

    [Space(25)]

    [Header("Attack Settings")]
    [SerializeField] [Tooltip("Can the AI trigger its attack event?")] public bool canAttack = false;
    [SerializeField] [Tooltip("Object Layers that are considered Attack Objects")] public LayerMask attackMask;
    [SerializeField] [Tooltip("Does the AI always trigger its attack event, even when it cannot see an attack object?")] public bool alwaysAttacks = false;
    [SerializeField] [Tooltip("Does the AI only attack objects in face direction?")] public bool attackOnlyWhenInFaceDirection = false;
    [SerializeField] [Tooltip("Does the AI attack only in the direction of the attack object?")] public bool attackOnlyInDirectionOfAttackObject = false;
    [SerializeField] [Tooltip("Time between when an AI triggers its attack.")] public float timeBetweenAttacks = .5f;
    [SerializeField] [Tooltip("Distance attack object must be away to attack it.")] public float attackDistance;
    [Space(5)]
    [SerializeField] UnityEvent m_AttackRight;
    [SerializeField] UnityEvent m_AttackLeft;


    //Component References
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private Transform _characterBase;
    private GameObject _chaseObject;

    //Private Variables
    private float faceDirection = 1f; //1 = Right, -1 = Left
    private int activeState = -1; //0 = Idle, 1 = Patrol, 2 = Chase
    private int nextState = 0; //0 = Idle, 1 = Patrol, 2 = Chase
    private bool isGrounded = false;
    private bool isForgetting = false;
    

    //Timers
    private float stateTimer = 0f;
    private float attackTimer = 0f;

    private void Start_GetReferences()
    {
        string missingComponents = "";
        if (!TryGetComponent(out _rigidbody2D))
        {
            missingComponents += "Rigidbody2D,";
        }
        if (!TryGetComponent(out _animator))
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
            {
                missingComponents += "Animator,";
            }
        }
        if (!TryGetComponent(out _collider))
        {
            missingComponents += "CapsuleCollider2D,";
        }

        _characterBase = this.transform.Find("CharacterBase");
        if (_characterBase == null)
        {
            Debug.LogError(this.gameObject.name + " is missing required child 'CharacterBase'. This child is used to determine if character is grounded and should be placed at the 'feet' of the character.");
        }

        if (missingComponents.Length > 0)
        {
            missingComponents = missingComponents.Trim(','); //Trim the last comma off.
            Debug.LogError(this.gameObject.name + " is missing the follow required components/references for its 'AIMovement' component (" + missingComponents + ")");
        }
    }

    private void Start_SetInitialState()
    {
        activeState = -1;
        if (canIdle)
        {
            nextState = 0;
        }
        else if (canPatrol)
        {
            nextState = 1;
        }
        else
        {
            Debug.Log(this.gameObject.name + " AIMovement component must have at least the ability to patrol or idle.");
        }
    }

    private void Start()
    {
        Start_GetReferences();
        Start_SetInitialState();
    }

    private void FixedUpdate()
    {
        if (activeState != nextState)
        {
            switch (nextState)
            {
                case 0: //Idle
                    State_Enter_Idle();
                    break;
                case 1: //Patrol
                    State_Enter_Patrol();
                    break;
                case 2: //Chase
                    State_Enter_Chase();
                    break;
            }
        }

        switch (activeState)
        {
            case 0: //Idle
                State_Idle();
                break;
            case 1: //Patrol
                State_Patrol();
                break;
            case 2: //Chase
                State_Chase();
                break;
        }

        CheckGrounded();
        _animator.SetFloat("_xVelocity", _rigidbody2D.velocity.x);
        _animator.SetBool("_isRunning", Mathf.Abs(_rigidbody2D.velocity.x) > 0);
        _animator.SetFloat("_yVelocity", _rigidbody2D.velocity.y);

        CanSeeChaseObject();
        Attack();

        if (activeState != nextState)
        {
            switch (activeState)
            {
                case 0: //Idle
                    State_Exit_Idle();
                    break;
                case 1: //Patrol
                    State_Exit_Patrol();
                    break;
                case 2: //Chase
                    State_Exit_Chase();
                    break;
            }
        }
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
            if (isGrounded == false) m_Landed.Invoke();
            isGrounded = true;

            if (_animator)
            {
                _animator.SetBool("_isGrounded", true);
            }
        }
        else
        {
            if (_animator)
            {
                _animator.SetBool("_isGrounded", false);
                isGrounded = false;
            }
        }
    }

    private void State_Enter_Idle()
    {
        if (!canIdle) return;

        activeState = 0;

        //Determine how long AI will idle.
        var idleTime = Random.Range(idleTimeMin, idleTimeMax);
        stateTimer = idleTime;

        m_IdleStarted.Invoke();
    }

    private void State_Idle()
    {
        //Count down timer
        stateTimer -= Time.deltaTime;

        //Check if we should enter the patrol state
        if (stateTimer <= 0)
        {
            if (canPatrol)
            {
                nextState = 1;
            }
        }
    }

    private void State_Exit_Idle()
    {
        m_IdleEnded.Invoke();
    }

    private void State_Enter_Patrol()
    {
        if (!canPatrol) return;
        if (changeDirection) FlipDirection();

        activeState = 1;

        //Determine how long AI will patrol.
        var patrolTime = Random.Range(patrolTimeMin, patrolTimeMax);
        stateTimer = patrolTime;

        m_PatrolStarted.Invoke();
    }

    private void State_Patrol()
    {
        //Count down timer
        stateTimer -= Time.deltaTime;

        //Check if we should enter the idle state
        if (stateTimer <= 0)
        {
            if (canIdle)
            {
                nextState = 0;
            }
        }

        //Move the AI toward its face direction at patrol speed.
        _rigidbody2D.velocity = new Vector2(patrolSpeed * faceDirection, _rigidbody2D.velocity.y);
    }

    private void State_Exit_Patrol()
    {
        m_PatrolEnded.Invoke();
    }

    private void State_Enter_Chase()
    {
        if (!canChase) return;

        activeState = 2;
        stateTimer = timeToForget;
        isForgetting = false;
        m_ChaseStarted.Invoke();
    }

    private void State_Chase()
    {
        //Move the AI toward the player
        _rigidbody2D.velocity = new Vector2(chaseSpeed * faceDirection, _rigidbody2D.velocity.y);

        float distanceFromChaseObject = Vector3.Distance(_chaseObject.transform.position, this.transform.position);
        if (distanceFromChaseObject < followDistance)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }

        if (distanceFromChaseObject > 5)
        {
            //Lost Sight of the chase object.
            isForgetting = true;
        }

        if (isForgetting)
        {
            stateTimer -= Time.deltaTime;
        }

        if (stateTimer <= 0)
        {
            if (canIdle)
            {
                nextState = 0; //Idle
            } else
            {
                nextState = 1; //Patrol
            }
        }
    }

    private void State_Exit_Chase()
    {
        isForgetting = false;
        m_ChaseEnded.Invoke();
    }

    private void CanSeeChaseObject()
    {
        RaycastHit2D faceDirectionCheck;
        RaycastHit2D behindDirectionCheck;
        Collider2D collider = null;
        

        if (sightFaceDirectionOnly)
        {
            faceDirectionCheck = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, sightDistance, chaseMask);
            Debug.DrawRay(transform.position, (Vector2.right * faceDirection) * sightDistance, Color.red);

            if (faceDirectionCheck.collider != null)
            {
                collider = faceDirectionCheck.collider;
            }
        } else
        {
            faceDirectionCheck = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, sightDistance, chaseMask);
            behindDirectionCheck = Physics2D.Raycast(transform.position, Vector2.right * -faceDirection, sightDistance, chaseMask);
            Debug.DrawRay(transform.position, (Vector2.right * faceDirection) * sightDistance, Color.red);
            Debug.DrawRay(transform.position, (Vector2.right * -faceDirection) * sightDistance, Color.red);

            if (faceDirectionCheck.collider != null)
            {
                collider = faceDirectionCheck.collider;
            }
            else if (behindDirectionCheck.collider != null)
            {
                collider = behindDirectionCheck.collider;
            }

            
        }

        if (collider != null)
        {
            if (collider.tag != "World" || canSeeThroughWalls)
            {
                nextState = 2; //Chase State
                _chaseObject = collider.gameObject;
            }
        }

        if (_chaseObject != null && !isForgetting)
        {
            float chaseObjectDirection = 1;
            if (_chaseObject.transform.position.x < this.transform.position.x) chaseObjectDirection = -1;
            FaceDirection(chaseObjectDirection);
        }
    }

    private void Attack()
    {
        if (!canAttack) return;

        attackTimer -= Time.deltaTime;

        RaycastHit2D faceDirectionCheck;
        RaycastHit2D behindDirectionCheck;
        float attackDirection = 0;

        if (!alwaysAttacks)
        {
            if (attackOnlyWhenInFaceDirection)
            {
                faceDirectionCheck = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, attackDistance, attackMask);
                Debug.DrawRay(transform.position, (Vector2.right * faceDirection) * sightDistance, Color.black);

                if (faceDirectionCheck.collider != null)
                {
                    attackDirection = faceDirection;
                }
            }
            else
            {
                faceDirectionCheck = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, attackDistance, attackMask);
                behindDirectionCheck = Physics2D.Raycast(transform.position, Vector2.right * -faceDirection, attackDistance, attackMask);
                Debug.DrawRay(transform.position, (Vector2.right * faceDirection) * sightDistance, Color.black);
                Debug.DrawRay(transform.position, (Vector2.right * -faceDirection) * sightDistance, Color.black);

                if (faceDirectionCheck.collider != null)
                {
                    attackDirection = faceDirection;
                }
                else if (behindDirectionCheck.collider != null)
                {
                    attackDirection = -faceDirection;
                }
            }
        }
        

        if (attackTimer <= 0)
        {
            if (attackOnlyInDirectionOfAttackObject)
            {
                if (attackDirection == 1 || alwaysAttacks)
                {
                    m_AttackRight.Invoke();

                    attackTimer = timeBetweenAttacks;
                }
                else if (attackDirection == -1 || alwaysAttacks)
                {
                    m_AttackLeft.Invoke();

                    attackTimer = timeBetweenAttacks;
                }
            }
            else
            {
                if (attackDirection != 0 || alwaysAttacks)
                {
                    m_AttackRight.Invoke();
                    m_AttackLeft.Invoke();

                    attackTimer = timeBetweenAttacks;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Check if it has a Percent Chance component
        PercentChance percentChance = null;
        bool success = true;
        
        if (other.TryGetComponent(out percentChance))
        {
            success = percentChance.GetSuccess();
        }

        if (success)
        {
            switch (other.tag)
            {
                case "AIJump":
                    CommandJump();
                    break;
                case "AITurn":
                    CommandTurn();
                    break;
            }
        }
    }

    public void CommandJump()
    {
        if (!canJump) return;
        if (activeState == 0) return;
        if (!isGrounded) return;

        _rigidbody2D.AddForce(new Vector2(0, jumpForce));
        m_Jumped.Invoke();
    }

    public void CommandTurn()
    {
        if (!canTurn) return;
        if (activeState != 1 || isForgetting) return; //Can only turn around if in Patrol state.

        FlipDirection();

        if (idleAfterTurn) nextState = 0;

        m_Turned.Invoke();
    }

    private void FlipDirection()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        faceDirection = Mathf.Sign(transform.localScale.x);
        _animator.SetFloat("_faceDirection", faceDirection);
    }

    private void FaceDirection(float direction)
    {
        Debug.Log("Face Direction: " + direction);
        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        faceDirection = Mathf.Sign(transform.localScale.x);
        _animator.SetFloat("_faceDirection", faceDirection);
    }
}
