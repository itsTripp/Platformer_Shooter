using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class Throwable : MonoBehaviour
    {
        [SerializeField]
        public float throwSpeed;
        [SerializeField]
        public float throwSpin;

        private Rigidbody2D _rigidbody2D;

        public void Throw(int equipFactor)
        {
            if (!this.TryGetComponent(out _rigidbody2D)) return;

            Vector2 throwDirection = new Vector2(equipFactor, .5f);
            _rigidbody2D.AddForce(throwDirection * throwSpeed);
            _rigidbody2D.AddTorque(equipFactor * throwSpin);
        }
    }
}

