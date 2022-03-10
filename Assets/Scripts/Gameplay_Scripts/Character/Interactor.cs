using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EpicTortoiseStudios
{
    public class Interactor : MonoBehaviour
    {
        public Weapon _selectedWeapon;

        private Interactable selectedInteractable = null;
        private List<Interactable> interactables = new List<Interactable>();
        private int selectedInteractIndex = -1;

        public void AddInteractable(Interactable interact)
        {
            interactables.Add(interact);
            SelectInteractable(interactables.Count - 1);
        }

        public void RemoveInteractable(Interactable interact)
        {
            interact.UnselectInteractable();
            interactables.Remove(interact);
            SelectNextInteractable();
        }

        public void RemoveSelectedInteractable()
        {
            if (selectedInteractable != null)
            {
                selectedInteractable.UnselectInteractable();
                interactables.Remove(selectedInteractable);
                SelectNextInteractable();
            }
        }

        public void UseSelectedInteractable()
        {
            selectedInteractable.UseSelected();
            RemoveSelectedInteractable();
        }

        public void SelectInteractable(int index)
        {
            if (index >= interactables.Count) index = 0; //First Index
            if (index < 0) index = interactables.Count - 1; //Last Index

            if (interactables.Count - 1 >= index)
            {
                //Unselect Previous Interactable
                if (selectedInteractable != null)
                {
                    selectedInteractable.UnselectInteractable();
                }

                //Select the new interactable
                selectedInteractIndex = index;
                selectedInteractable = interactables[selectedInteractIndex];
                selectedInteractable.SelectInteractable();

                //Check interactable type

                //If Interactable is on a gameobject with Weapon then set the Weapon as the interactable.
                if (selectedInteractable.TryGetComponent(out _selectedWeapon)) return;
            } else
            {
                selectedInteractable = null;
                _selectedWeapon = null;
            }
        }

        public void SelectNextInteractable()
        {
            SelectInteractable(selectedInteractIndex + 1);
        }

        public void SelectPrevInteractable()
        {
            SelectInteractable(selectedInteractIndex - 1);
        }
    }
}