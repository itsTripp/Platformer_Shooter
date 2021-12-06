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
        [SerializeField]
        private GameObject optionsMenu;
        [SerializeField]
        private GameObject creditsMenu;

        // Start is called before the first frame update
        void Start()
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape) && creditsMenu.activeSelf)
            {
                creditsMenu.SetActive(false);
            }
        }

        public void Play_Game()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Options_Menu()
        {
            optionsMenu.SetActive(true);
        }

        public void Credits_Menu()
        {
            creditsMenu.SetActive(true);
        }

        public void Back_Button()
        {
            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
            }
            if (creditsMenu.activeSelf)
            {
                creditsMenu.SetActive(false);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
            ///Comment This out before release
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
