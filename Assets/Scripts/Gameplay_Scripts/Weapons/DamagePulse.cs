using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EpicTortoiseStudios
{
    public class DamagePulse : Damage
    {
        [SerializeField] float _pulseTime = .25f;
        [SerializeField] float _pulseActiveTime = .05f;
        [SerializeField] Collider2D _trigger;

        private float _currentTime = 0f;

        [Header("Events")]
        [SerializeField] UnityEvent m_Activate;
        [SerializeField] UnityEvent m_Deactivate;

        private void FixedUpdate()
        {
            if (_trigger == null) return;

            _currentTime += Time.deltaTime;
            if (_currentTime > _pulseTime)
            {
                if (!_trigger.enabled)
                {
                    m_Activate.Invoke();
                    _trigger.enabled = true;
                }

                //Activate Trigger
                if (_currentTime > _pulseTime + _pulseActiveTime)
                {
                    //Deactivate Trigger
                    m_Deactivate.Invoke();
                    _trigger.enabled = false;
                    _currentTime = 0f;
                }
            }
        }
    }
}
