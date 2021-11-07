using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed;
    [SerializeField]
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody.velocity = transform.up * _projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 11f)
        {
            Destroy(gameObject);
        }
        if(transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }
}
