using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField]
        private int _pickupID;
        [SerializeField]
        private AudioClip _audioClip;
        private Player _player;
        public LootPickups lootPickup;



        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player == null)
            {
                Debug.LogError("Player is Null on PickupItem");
            }
        }


        // Update is called once per frame
        void Update()
        {
            //Screen Wrapping
            if (transform.position.x > 10.3f)
            {
                transform.position = new Vector3(-10.3f, transform.position.y, 0);
            }
            if (transform.position.x < -10.3f)
            {
                transform.position = new Vector3(10.3f, transform.position.y, 0);
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
    }
}
