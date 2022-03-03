using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace EpicTortoiseStudios
{
    public class MainMenu : MonoBehaviour
    {
        public PlayerController controller;

        [SerializeField]
        private GameObject mainMenu;
        [SerializeField]
        private GameObject settingsMenu;
        [SerializeField]
        private GameObject displaySettings;
        [SerializeField]
        private GameObject audioSettings;
        [SerializeField]
        private GameObject inputSettings;
        [SerializeField]
        private GameObject keyboardSettings;
        [SerializeField]
        private GameObject controllerSettings;
        [SerializeField]
        private GameObject achievementsMenu;
        [SerializeField]
        private GameObject leaderboardMenu;
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
        private GameObject inputSettingsFirstButton;
        [SerializeField]
        private GameObject inputSettingsClosedButton;
        [SerializeField]
        private GameObject keyboardSettingsFirstButton;
        [SerializeField]
        private GameObject keyboardSettingsClosedButton;
        [SerializeField]
        private GameObject controllerSettingsFirstButton;
        [SerializeField]
        private GameObject controllerSettingsClosedButton;
        [SerializeField]
        private GameObject audioSettingsFirstButton;
        [SerializeField]
        private GameObject audioSettingsClosedButton;
        [SerializeField]
        private GameObject achievementsFirstButton;
        [SerializeField]
        private GameObject achievementsClosedButton;
        [SerializeField]
        private GameObject leaderboardFirstButton;
        [SerializeField]
        private GameObject leaderboardClosedButton;

        [SerializeField]
        private AudioSource audioSource;


        // Start is called before the first frame update
        void Start()
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);
            displaySettings.SetActive(false);
            audioSettings.SetActive(false);
            inputSettings.SetActive(false);
            keyboardSettings.SetActive(false);
            controllerSettings.SetActive(false);
            achievementsMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            /*if (mainMenu.activeSelf == true)
            {
                UIManager.UIManagerInstance.gameObject.SetActive(false);
            }*/
        }

        private void Awake()
        {
            controller = new PlayerController();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Play_Game()
        {
            audioSource.Play();
            StartCoroutine(PlayGameWithDelay());
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        IEnumerator PlayGameWithDelay()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Application.Quit();
            ///Comment This out before release
            UnityEditor.EditorApplication.isPlaying = false;
        }

        public void Settings_Menu()
        {
            settingsMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        }

        public void DisplaySettings()
        {
            displaySettings.SetActive(true);
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(displaySettingsFirstButton);
        }
        public void AudioSettings()
        {
            audioSettings.SetActive(true);
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(audioSettingsFirstButton);
        }

        public void InputSettings()
        {
            inputSettings.SetActive(true);
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(inputSettingsFirstButton);
        }
        public void KeyboardSettings()
        {
            keyboardSettings.SetActive(true);
            inputSettings.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(keyboardSettingsFirstButton);
        }

        public void ControllerSettings()
        {
            controllerSettings.SetActive(true);
            inputSettings.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(controllerSettingsFirstButton);
        }

        public void AchievementsMenu()
        {
            achievementsMenu.SetActive(true);
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(achievementsFirstButton);
        }

        public void LeaderboardsMenu()
        {
            leaderboardMenu.SetActive(true);
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(leaderboardFirstButton);
        }

        public void Back_Button()
        {
            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(settingsClosedButton);
            }
            if (displaySettings.activeSelf)
            {
                displaySettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(displaySettingsClosedButton);
            }
            if (audioSettings.activeSelf)
            {
                audioSettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(audioSettingsClosedButton);
            }
            if (inputSettings.activeSelf)
            {
                inputSettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(inputSettingsClosedButton);
            }
            if (keyboardSettings.activeSelf)
            {
                keyboardSettings.SetActive(false);
                inputSettings.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(keyboardSettingsClosedButton);
            }
            if (controllerSettings.activeSelf)
            {
                controllerSettings.SetActive(false);
                inputSettings.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(controllerSettingsClosedButton);
            }
            if (achievementsMenu.activeSelf)
            {
                achievementsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(achievementsClosedButton);
            }
            if (leaderboardMenu.activeSelf)
            {
                leaderboardMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(leaderboardClosedButton);
            }
            
        }
        public void Cancel(InputAction.CallbackContext context)
        {
            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(settingsClosedButton);
            }
            if (displaySettings.activeSelf)
            {
                displaySettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(displaySettingsClosedButton);
            }
            if (audioSettings.activeSelf)
            {
                audioSettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(audioSettingsClosedButton);
            }
            if (inputSettings.activeSelf)
            {
                inputSettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(inputSettingsClosedButton);
            }
            if (keyboardSettings.activeSelf)
            {
                keyboardSettings.SetActive(false);
                inputSettings.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(keyboardSettingsClosedButton);
            }
            if (controllerSettings.activeSelf)
            {
                controllerSettings.SetActive(false);
                inputSettings.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(controllerSettingsClosedButton);
            }
            if (achievementsMenu.activeSelf)
            {
                achievementsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(achievementsClosedButton);
            }
            if (leaderboardMenu.activeSelf)
            {
                leaderboardMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(leaderboardClosedButton);
            }
        }
    }
}
