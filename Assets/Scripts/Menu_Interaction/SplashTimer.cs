using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SplashScreenTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SplashScreenTimer()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Main_Menu");
    }
}
