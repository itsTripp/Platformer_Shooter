using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace EpicTortoiseStudios
{

    public class PauseMenu : MonoBehaviour
    {
        public PlayerController controller;
        
        public static PauseMenu pauseInstance;
        public static bool GameIsPaused = false;

        [SerializeField]
        private GameObject pauseMenu;
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
        [SerializeField]
        private GameObject creditsMenu;
        [SerializeField]
        private GameObject quitMenu;
        [Header("First Buttons")]
        [SerializeField]
        private GameObject pauseMenuFirstButton;
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
        private GameObject keyboardSettingsFirstButton;
        [SerializeField]
        private GameObject keyboardSettingsClosedButton;
        [SerializeField]
        private GameObject controllerSettingsFirstButton;
        [SerializeField]
        private GameObject controllerSettingsClosedButton;
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
            settingsMenu.SetActive(false);
            displaySettings.SetActive(false);
            audioSettings.SetActive(false);
            inputSettings.SetActive(false);
            keyboardSettings.SetActive(false);
            controllerSettings.SetActive(false);
            achievementsMenu.SetActive(false);
            leaderboardMenu.SetActive(false);
            creditsMenu.SetActive(false);
            DontDestroyOnLoad(this);
            if(pauseInstance == null)
            {
                pauseInstance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Awake()
        {
            controller = new PlayerController();
        }

        // Update is called once per frame
        void Update()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            /*if(Input.GetKeyDown(pauseInput) && sceneName != "Main_Menu")
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            if(Input.GetKeyDown(pauseInput) && pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(pauseInput))
            {
                if (sceneName != "Main_Menu" && GameIsPaused == false)
                {
                    PauseGame();
                }
                else
                {
                    UnPauseGame();
                }
            }*/
        }

        public void PauseGame(InputAction.CallbackContext context)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            if(sceneName != "Main_Menu")
            {
                if (pauseMenu.activeSelf == false)
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;
                    GameIsPaused = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(pauseMenuFirstButton);
                    UIManager.UIManagerInstance._waveText.gameObject.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                    GameIsPaused = false;
                }
            }
            
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

        public void Credits_Menu()
        {
            creditsMenu.SetActive(true);
            settingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsFirstButton);
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main_Menu");
            quitMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void QuitMenu()
        {
            quitMenu.SetActive(true);
            pauseMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(quitMenuFirstButton);
        }

        public void Back_Button()
        {
            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(settingsClosedButton);
            }
            if(displaySettings.activeSelf)
            {
                displaySettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(displaySettingsClosedButton);
            }
            if(audioSettings.activeSelf)
            {
                audioSettings.SetActive(false);
                settingsMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(audioSettingsClosedButton);
            }
            if(inputSettings.activeSelf)
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
            if(leaderboardMenu.activeSelf)
            {
                leaderboardMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(leaderboardClosedButton);
            }
            if(creditsMenu.activeSelf)
            {
                creditsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(creditsClosedButton);
            }
            if(quitMenu.activeSelf)
            {
                quitMenu.SetActive(false);
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(quitMenuClosedButton);
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
            if (creditsMenu.activeSelf)
            {
                creditsMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(creditsClosedButton);
            }
            if (quitMenu.activeSelf)
            {
                quitMenu.SetActive(false);
                pauseMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(quitMenuClosedButton);
            }
        }
    }
}