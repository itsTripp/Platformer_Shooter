using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EpicTortoiseStudios
{
    public class Grenade : Throwable
    {
        [SerializeField]
        private float explodeAfterSeconds = 5f;
        [SerializeField]
        private int explodeAfterImpacts = -1;
        [SerializeField]
        private Transform explosion;


        private int numOfImpacts = 0;


        private void Awake()
        {
            if (explodeAfterSeconds > 0)
            {
                StartCoroutine(Countdown());
            }
        }

        IEnumerator Countdown()
        {
            yield return new WaitForSeconds(explodeAfterSeconds);

            Explode();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (explodeAfterImpacts > 0)
            {
                numOfImpacts += 1;
                if (numOfImpacts >= explodeAfterImpacts)
                {
                    Explode();
                }
            }
        }

        public void Explode()
        {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
    }
}