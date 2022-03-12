using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EpicTortoiseStudios
{
    public class Interactable : MonoBehaviour
    {


        [Header("Events")]
        [SerializeField] UnityEvent m_InteractableSelected;
        [SerializeField] UnityEvent m_InteractableUnselected;
        [SerializeField] UnityEvent m_Use;

        public void SelectInteractable()
        {
            m_InteractableSelected.Invoke();
        }

        public void UnselectInteractable()
        {
            m_InteractableUnselected.Invoke();
        }

        public void UseSelected() 
        {
            m_Use.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Interactor interactor = collision.GetComponent<Interactor>();
            if (interactor != null)
            {
                interactor.AddInteractable(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Interactor interactor = collision.GetComponent<Interactor>();
            if (interactor != null)
            {
                interactor.RemoveInteractable(this);
            }
        }
    }
}

