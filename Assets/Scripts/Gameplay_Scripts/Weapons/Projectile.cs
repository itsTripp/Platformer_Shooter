using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(gameObject);
        }
    }
}
