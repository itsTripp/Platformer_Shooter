using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class SpawnPoint : MonoBehaviour
    {
        [ReadOnly] public bool _active = true;
        [SerializeField] public bool _hideSpriteOnStart = true;
        [SerializeField] public float _disableDistance = 0f;

        private SpriteRenderer _spriteRenderer;
        private GameObject _playerGameObject;

        private void Start()
        {
            if (TryGetComponent<SpriteRenderer>(out _spriteRenderer) && _hideSpriteOnStart)
            {
                _spriteRenderer.enabled = false;
            }

            _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        }

        private void FixedUpdate()
        {
            float distToPlayer = Vector3.Distance(_playerGameObject.transform.position, this.transform.position);

            if (distToPlayer < _disableDistance)
            {
                _active = false;

                if (_spriteRenderer != null)
                {
                    _spriteRenderer.color = Color.red;
                }
            } else
            {
                _active = true;

                if (_spriteRenderer != null)
                {
                    _spriteRenderer.color = Color.green;
                }
            }
        }
    }
}

