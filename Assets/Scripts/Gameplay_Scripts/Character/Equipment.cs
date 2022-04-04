using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace EpicTortoiseStudios
{
    public class Equipment : MonoBehaviour
    {
        [Header("Equip Locations")]
        [SerializeField] private Transform _rightEquipLocation;
        [SerializeField] private Transform _leftEquipLocation;

        [Header("Equipped Equipment")]
        [SerializeField] private Weapon _rightWeapon;
        [SerializeField] private Weapon _leftWeapon;
        [SerializeField] private Throwable _throwable;
        [SerializeField] private object _activeSkill;

        [Header("Equipment")]
        [SerializeField] private Throwable[] _throwableList;
        [SerializeField] private object[] _activeSkillList;

        private Inventory _inventory;
        private Interactor _interactor;
        private Health _health;

        private int _leftWeaponOrder = 4;
        private SpriteRenderer _leftWeaponSpriteRenderer;

        private bool _attackHeldRight = false;
        private bool _attackHeldLeft = false;

        [Header("Events")]
        [SerializeField] UnityEvent m_Equip;
        [SerializeField] UnityEvent m_Unequip;
        [SerializeField] UnityEvent m_Throw;
        [SerializeField] UnityEvent m_Attack;

        void Start()
        {
            string missingComponents = "";
            if (_rightEquipLocation == null)
            {
                missingComponents += "Right Equip Location,";
            }
            if (_leftEquipLocation == null)
            {
                missingComponents += "Left Equip Location,";
            }
            if (!TryGetComponent(out _inventory))
            {
                missingComponents += "Inventory,";
            }
            _interactor = this.transform.GetComponentInChildren<Interactor>(true);
            //if (_interactor == null)
            //{
            //    missingComponents += "Interactor,";
            //}
            if (!TryGetComponent(out _health))
            {
                missingComponents += "Health,";
            }

            if (missingComponents.Length > 0)
            {
                missingComponents = missingComponents.Trim(','); //Trim the last comma off.
                Debug.LogError(this.gameObject.name + " is missing the follow required components/references for its 'Equipment' component (" + missingComponents + ")");
            }

            if (_rightWeapon && _rightEquipLocation && _inventory && _health)
            {
                _rightWeapon.EquipToCharacter(_rightEquipLocation, this.gameObject, _inventory, _health);
            }

            if (_leftWeapon && _leftEquipLocation && _inventory && _health)
            {
                _leftWeapon.EquipToCharacter(_leftEquipLocation, this.gameObject, _inventory, _health);
            }
            
        }

        private void Update()
        {
            if (_attackHeldRight)
            {
                AttackRight();
            }

            if (_attackHeldLeft)
            {
                AttackLeft();
            }
        }

        public void EquipRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_rightWeapon)
                {
                    _rightWeapon.UnequipFromCharacter();
                    _interactor.RemoveSelectedInteractable();
                    _rightWeapon = null;

                    m_Unequip.Invoke();
                }

                if (_interactor != null && _interactor._selectedWeapon != null)
                {
                    _rightWeapon = _interactor._selectedWeapon;
                    _interactor._selectedWeapon = null;
                    _interactor.RemoveSelectedInteractable();
                    _rightWeapon.EquipToCharacter(_rightEquipLocation, this.gameObject, _inventory, _health);

                    m_Equip.Invoke();
                }
            }
        }

        public void UnequipAll(int percent)
        {
            int randomVal = Random.Range(0, 100);
            Debug.Log("Random Value: " + randomVal);
            if (randomVal <= percent)
            {
                if (_rightWeapon)
                {
                    _rightWeapon.UnequipFromCharacter();
                    _rightWeapon = null;

                    m_Unequip.Invoke();
                }

                if (_leftWeapon)
                {
                    _leftWeapon.UnequipFromCharacter();
                    _leftWeapon = null;

                    m_Unequip.Invoke();
                }
            } else
            {
                if (_rightWeapon)
                {
                    Debug.Log("Destroy Right Weapon");
                    Destroy(_rightWeapon.gameObject);
                    _rightWeapon = null;
                }

                if (_leftWeapon)
                {
                    Debug.Log("Destroy Left Weapon");
                    Destroy(_leftWeapon.gameObject);
                    _leftWeapon = null;
                }
            }
        }

        public void EquipLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_leftWeapon)
                {
                    _leftWeapon.UnequipFromCharacter();
                    _leftWeapon = null;

                    m_Unequip.Invoke();
                }

                if (_interactor != null && _interactor._selectedWeapon != null)
                {
                    _leftWeapon = _interactor._selectedWeapon;
                    _interactor._selectedWeapon = null;
                    _interactor.RemoveSelectedInteractable();
                    _leftWeapon.EquipToCharacter(_leftEquipLocation, this.gameObject, _inventory, _health);

                    _leftWeapon.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    _leftWeaponSpriteRenderer.sortingOrder = _leftWeaponOrder; // Not Currently Working

                    m_Equip.Invoke();
                }
            }
        }

        public void ThrowableRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_throwable) // Does the character have a weapon in its left hand?
                {
                    ThrowThrowable(_rightEquipLocation, 1);
                }
            }
        }

        public void ThrowableLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_throwable) // Does the character have a weapon in its left hand?
                {
                    ThrowThrowable(_leftEquipLocation, -1);
                }
            }
        }

        private void ThrowThrowable(Transform spawnLocation, int direction)
        {
            Throwable instThrowable = Instantiate(_throwable, spawnLocation.position, spawnLocation.rotation);
            instThrowable.Throw(direction);

            m_Throw.Invoke();
        }

        private void AttackRight()
        {
            if (_rightWeapon) // Does the character have a weapon in its left hand?
            {
                _rightWeapon.Fire(); //Send fire command to left equipped weapon

                m_Attack.Invoke();
            }
        }

        private void AttackLeft()
        {
            if (_leftWeapon) // Does the character have a weapon in its left hand?
            {
                _leftWeapon.Fire(); //Send fire command to left equipped weapon

                m_Attack.Invoke();
            }
        }

        public void AttackRightInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _attackHeldRight = true;
            }

            if (context.canceled)
            {
                _attackHeldRight = false;
            }
        }

        public void AttackLeftInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _attackHeldLeft = true;
            }

            if (context.canceled)
            {
                _attackHeldLeft = false;
            }
        }
    }

}
