using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class VelocityDamage : Damage
    {
        [SerializeField]
        public float minVelocity;
        [SerializeField]
        public float maxVelocity;

        private float maxDamage;
        private float maxKnock;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = transform.GetComponent<Rigidbody2D>();
            maxDamage = damage;
            maxKnock = knock;
        }

        private void FixedUpdate()
        {
            if (_rigidbody)
            {
                if (_rigidbody.velocity.magnitude >= minVelocity)
                {
                    float percVelocity = Mathf.Clamp(_rigidbody.velocity.magnitude / maxVelocity, 0, 1);
                    damage = maxDamage * percVelocity;
                    knock = maxKnock * percVelocity;
                } else
                {
                    damage = 0;
                    knock = 0;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _rigidbody.velocity = new Vector2(0, 0);
        }
    }
}
