using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [ExecuteInEditMode()]
    public class ModularUI : MonoBehaviour
    {
        public ModularUI_Data skinData;

        protected virtual void OnSkinUI()
        {

        }

        public virtual void Awake()
        {
            OnSkinUI();
        }

        public virtual void Update()
        {
            if (Application.isEditor)
            {
                OnSkinUI();
            }
        }
    }
}
