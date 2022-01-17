using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace EpicTortoiseStudios
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenu;
        [Header("First Buttons")]
        [SerializeField]
        private GameObject mainMenuFirstButton;
        [SerializeField]
        private GameObject settingsFirstButton;
        [SerializeField]
        private GameObject settingsClosedButton;
        [SerializeField]
        private GameObject displaySettingsFirstButton;
        [SerializeField]
        private GameObject displaySettingsClosedButton;
        [SerializeField]
        private GameObject audioSettingsFirstButton;
        [SerializeField]
        private GameObject audioSettingsClosedButton;
        [SerializeField]
        private GameObject inputSettingsFirstButton;
        [SerializeField]
        private GameObject inputSettingsClosedButton;
        [SerializeField]
        private GameObject achievementsFirstButton;
        [SerializeField]
        private GameObject achievementsClosedButton;
        [SerializeField]
        private GameObject leaderboardFirstButton;
        [SerializeField]
        private GameObject leaderboardClosedButton;
        [SerializeField]
        private GameObject creditsFirstButton;
        [SerializeField]
        private GameObject creditsClosedButton;
        [SerializeField]
        private GameObject quitMenuFirstButton;
        [SerializeField]
        private GameObject quitMenuClosedButton;


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
