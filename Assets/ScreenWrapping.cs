using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
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
