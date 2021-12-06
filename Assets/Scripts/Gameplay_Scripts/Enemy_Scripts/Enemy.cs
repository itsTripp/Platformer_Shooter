using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class Enemy : ScriptableObject
    {
        public Sprite enemySprite;
        public int enemyHealth;
        public float enemyMovementSpeed;

        public void EnemyMovement(Transform transform)
        {
            transform.Translate(Vector3.right * enemyMovementSpeed * Time.deltaTime);

            //Screen Wrapping
            if (transform.position.x > 11f)
            {
                transform.position = new Vector3(-11f, transform.position.y, 0);
            }
            if (transform.position.x < -11f)
            {
                transform.position = new Vector3(11f, transform.position.y, 0);
            }
            if (transform.position.y < -1.5f)
            {
                transform.position = new Vector3(transform.position.x, 11f, 0);
            }
            if (transform.position.y > 11f)
            {
                transform.position = new Vector3(transform.position.x, -1.5f, 0);
            }
        }
    }
}
