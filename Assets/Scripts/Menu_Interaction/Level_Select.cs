using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Select : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLevel1()
    {
        StartCoroutine(PlayLevel1WithDelay());
    }
    IEnumerator PlayLevel1WithDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level_01");
    }

    public void PlayLevel2()
    {
        StartCoroutine(PlayLevel2WithDelay());
    }
    IEnumerator PlayLevel2WithDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level_02");
    }

    public void PlayLevel3()
    {
        StartCoroutine(PlayLevel3WithDelay());
    }
    IEnumerator PlayLevel3WithDelay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level_03");
    }
}
