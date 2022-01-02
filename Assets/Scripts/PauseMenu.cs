using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EpicTortoiseStudios
{

    public class PauseMenu : MonoBehaviour
    {
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
        private GameObject achievementsMenu;
        [SerializeField]
        private GameObject leaderboardMenu;
        [SerializeField]
        private GameObject creditsMenu;
        [SerializeField]
        private GameObject quitMenu;

        [SerializeField]
        private KeyCode pauseInput;

        // Start is called before the first frame update
        void Start()
        {
            settingsMenu.SetActive(false);
            displaySettings.SetActive(false);
            audioSettings.SetActive(false);
            inputSettings.SetActive(false);
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

        // Update is called once per frame
        void Update()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            if(Input.GetKeyDown(pauseInput) && sceneName != "Main_Menu")
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
            }
        }

        private void PauseGame()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            GameIsPaused = true;
            UIManager.UIManagerInstance._waveText.gameObject.SetActive(false);
        }

        private void UnPauseGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            GameIsPaused = false;
        }
        public void Settings_Menu()
        {
            settingsMenu.SetActive(true);
        }

        public void DisplaySettings()
        {
            displaySettings.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void AudioSettings()
        {
            audioSettings.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void InputSettings()
        {
            inputSettings.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void AchievementsMenu()
        {
            achievementsMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void LeaderboardsMenu()
        {
            leaderboardMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }

        public void Credits_Menu()
        {
            creditsMenu.SetActive(true);
            settingsMenu.SetActive(false);
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
        }

        public void Back_Button()
        {
            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
            }
            if(displaySettings.activeSelf)
            {
                displaySettings.SetActive(false);
                settingsMenu.SetActive(true);
            }
            if(audioSettings.activeSelf)
            {
                audioSettings.SetActive(false);
                settingsMenu.SetActive(true);
            }
            if(inputSettings.activeSelf)
            {
                inputSettings.SetActive(false);
                settingsMenu.SetActive(true);
            }
            if(achievementsMenu.activeSelf)
            {
                achievementsMenu.SetActive(false);
            }
            if(leaderboardMenu.activeSelf)
            {
                leaderboardMenu.SetActive(false);
            }
            if(creditsMenu.activeSelf)
            {
                creditsMenu.SetActive(false);
            }
            if(quitMenu.activeSelf)
            {
                quitMenu.SetActive(false);
                pauseMenu.SetActive(true);
            }
        }
    }
}