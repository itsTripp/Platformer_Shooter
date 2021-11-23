using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_Manager : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    private bool _showCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(_canvas)
            {
                _showCanvas = !_showCanvas;
                _canvas.gameObject.SetActive(_showCanvas);
            }
        }
    }
}
