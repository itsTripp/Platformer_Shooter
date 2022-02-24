using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [System.Serializable]
    public class DamageResistance : MonoBehaviour
    {
        [SerializeField]
        public CommonEnums.DamageType type;
        [SerializeField]
        public float percent;
        [SerializeField]
        public bool canOverResist;

        public DamageResistance(CommonEnums.DamageType type, float percent, bool canOverResist)
        {
            this.type = type;
            this.percent = percent;
            this.canOverResist = canOverResist;
        }
    }
}