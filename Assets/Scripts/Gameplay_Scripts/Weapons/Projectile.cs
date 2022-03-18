using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EpicTortoiseStudios
{
    public class Projectile : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] UnityEvent m_TriggerCollided;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != this.gameObject.tag)
            {
                m_TriggerCollided.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
