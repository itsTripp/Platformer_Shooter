using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour
{
    private LevelManager _levelManager;

    private void Start()
    {
        GetLevelManager();    
    }

    private void GetLevelManager() {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > _levelManager.BoundaryRight)
        {
            transform.position = new Vector3(_levelManager.BoundaryLeft, transform.position.y, 0);
        }
        if (transform.position.x < _levelManager.BoundaryLeft)
        {
            transform.position = new Vector3(_levelManager.BoundaryRight, transform.position.y, 0);
        }
        if (transform.position.y < _levelManager.BoundaryBottom)
        {
            transform.position = new Vector3(transform.position.x, _levelManager.BoundaryTop, 0);
        }
        if (transform.position.y > _levelManager.BoundaryTop)
        {
            transform.position = new Vector3(transform.position.x, _levelManager.BoundaryBottom, 0);
        }
    }
}
