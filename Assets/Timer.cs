using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float _time = 0f;
    [SerializeField] bool _oneShot = true;
    [SerializeField] float _loopDelay = 0f;
    [SerializeField] bool _activateOnStart = false;

    [Header("Events")]
    [SerializeField] UnityEvent m_TimerActivated;
    [SerializeField] UnityEvent m_TimerDeactivated;
    [SerializeField] UnityEvent m_TimerStart;
    [SerializeField] UnityEvent m_TimerEnd;

    private bool _active;
    private bool _loopDelayActive;
    private float _currentTime;

    private void Start()
    {
        if (_activateOnStart)
        {
            _active = true;
            m_TimerActivated.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (_active)
        {
            
            if (_currentTime == 0f)
            {
                Debug.Log("Timer Start");
                m_TimerStart.Invoke();
            }

            _currentTime += Time.deltaTime;
            
            if (_currentTime > _time)
            {
                Debug.Log("Timer End");
                m_TimerEnd.Invoke();
                _currentTime = 0f;

                if (_oneShot)
                {
                    _active = false;
                } else
                {
                    _loopDelayActive = true;
                    _active = false;
                }
            }
        }

        if (_loopDelayActive)
        {
            if (_currentTime == 0f)
            {
                Debug.Log("Delay Start");
            }

            _currentTime += Time.deltaTime;

            if (_currentTime > _loopDelay)
            {
                Debug.Log("Delay End");
                _active = true;
                _loopDelayActive = false;
                _currentTime = 0f;
            }
        }
    }

    public void Activate()
    {
        _active = true;
        m_TimerActivated.Invoke();
    }

    public void ActivateWithTime(float _newTime)
    {
        _time = _newTime;
        _active = true;
        m_TimerActivated.Invoke();
    }

    public void DeActivate()
    {
        _active = false;
        m_TimerDeactivated.Invoke();
    }
}
