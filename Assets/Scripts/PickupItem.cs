using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField]
    private int _pickupID;
    [SerializeField]
    private AudioClip _audioClip;
    private Player _player;
    private PlayerPickups _playerPickups;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null on PickupItem");
        }
        _playerPickups = GameObject.Find("Player").GetComponent<PlayerPickups>();
        if (_playerPickups == null)
        {
            Debug.LogError("PlayerPickups is Null on PickupItem");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            switch (_pickupID)
            {
                case 0:
                    _playerPickups.AddAmmo();
                    break;
                case 1:
                    _playerPickups.AddExperience();
                    break;
                
            }
            Destroy(gameObject);
        }
    }
}
