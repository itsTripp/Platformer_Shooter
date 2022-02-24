using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [System.Serializable]
    public class Damage : MonoBehaviour
    {
        [SerializeField]
        public CommonEnums.DamageType type;
        [SerializeField]
        public float damage;
        [SerializeField]
        public float knock;
    }
}

