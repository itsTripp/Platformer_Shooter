using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class DamageRadius : Damage
    {
        [SerializeField]
        public float minDamage;
        [SerializeField]
        public float maxDamage;

        public float damageDifference;

        private CircleCollider2D _collider2D;
        private float maxDistance;

        private void Awake()
        {
            TryGetComponent(out _collider2D);
            damageDifference = maxDamage - minDamage;

            float radius = _collider2D.radius * this.transform.localScale.x;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
