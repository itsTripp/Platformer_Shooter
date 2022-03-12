using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class SpawnedObject : MonoBehaviour
    {
        public SpawnPool _spawnPool;
        public int _ticketCost;

        private void OnDestroy()
        {
            if (_spawnPool)
            {
                _spawnPool.EnemyDestroyed(this.gameObject, _ticketCost);
            }
        }
    }

}
