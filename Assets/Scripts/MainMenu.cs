using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EpicTortoiseStudios
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenu;
        

        // Start is called before the first frame update
        void Start()
        {
            mainMenu.SetActive(true);
            if(mainMenu.activeSelf == true)
            {
                UIManager.UIManagerInstance.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Play_Game()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Application.Quit();
            ///Comment This out before release
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
