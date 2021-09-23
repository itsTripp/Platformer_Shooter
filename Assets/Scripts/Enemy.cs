using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemyMovementSpeed = 4f;

    [SerializeField]
    private int _enemyHealth = 3;

    private Player _player;
    private EnemySpawnManager _enemySpawnManager;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _damageTakenAudio;

    private UIManager _uiManager;

    [Header("Loot")]
    [SerializeField]
    private Rigidbody2D _experiencePickup;
    [SerializeField]
    private float _upwardForce;
    [SerializeField]
    private float _outwardForce;
    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is Null on Enemy Script");
        }
        _enemySpawnManager = GameObject.Find("Enemy_Spawn_Manager").GetComponent<EnemySpawnManager>();
        if(_enemySpawnManager == null)
        {
            Debug.LogError("EnemySpawnManager is Null on Enemy Script");
        }
        _uiManager = GameObject.Find("Game_HUD").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is Null on Enemy Script");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source is Null on Enemy Script");
        }

        
}

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        transform.Translate(Vector3.right * _enemyMovementSpeed * Time.deltaTime);

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Player")
        {
            _enemyHealth--;
            _audioSource.PlayOneShot(_damageTakenAudio);
            if(_enemyHealth <= 0)
            {
                if(_player != null)
                {
                    _player.AddScore(10);
                }
                Destroy(gameObject,.2f);
                _enemySpawnManager.EnemyKilled();
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Aggrivation_Trigger")
        {
            Debug.Log("Enemy Hit The Trigger");
            _enemyMovementSpeed = 5f;
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }

        if(other.transform.tag == "Projectile")
        {
            _enemyHealth--;
            _audioSource.PlayOneShot(_damageTakenAudio);
            Destroy(other.gameObject);
            if(_enemyHealth <= 0)
            {
                if(_player != null)
                {
                    _player.AddScore(10);

                    int _experienceDrops = Random.Range(1, 4);
                    for (int i = 0; i <_experienceDrops; i++)
                    {
                        Rigidbody2D _expRig = Instantiate(_experiencePickup, transform.position, Quaternion.identity) as Rigidbody2D;
                        _expRig.AddForce(transform.up * _upwardForce);
                        _expRig.AddForce(transform.right * Random.Range(-_outwardForce, _outwardForce));
                    }
                }
                Destroy(gameObject, .1f);
                _enemySpawnManager.EnemyKilled();
            }
        }
    }
}