using Assets.Scripts.Cutscene;
using Assets.Scripts.GameSaveLoad;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        private bool isFadingToBridge;
        private bool isFadingToLevel;

        [SerializeField] private Transition transition;
        [SerializeField] private Text uiText;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button interventionButton;
        [SerializeField] private GameObject uiElements;

        public void Activate()
        {
            if (Game.GetGame().InLevel)
            {
                gameObject.SetActive(true);
            }
        }

        void Start()
        {
            // Level to Bridge
            if (settingsButton != null && infoButton != null && interventionButton != null)
            {
                ChangeInteractability(false);
            }
            else // Bridge to level
            {
                uiText.text += Game.GetGame().CurrentLevelNumber - 1 + "?";
            }
        }

        void Update()
        {
            if (!isFadingToBridge && !isFadingToLevel)
            {
                return;
            }

            if (!transition.transform.gameObject.activeSelf)
            {
                if (uiElements)
                {
                    uiElements.SetActive(false);
                }
                transition.transform.gameObject.SetActive(true);
            }

            if (!transition.FadeOut())
            {
                return;
            }

            SaveLoadGame.Save();

            if (isFadingToBridge)
            {
                // Load bridge
                SceneManager.LoadScene(GlobalVariablesHelper.BASEVIEW_SCENE_INDEX);
            }
            else // Fading to level
            {
                // Load current level
                Game game = Game.GetGame();
                SceneManager.LoadScene(game.CurrentLevelIndex);
            }
        }

        public void StartFadingToBridge()
        {
            isFadingToBridge = true;
        }

        public void StartFadingToLevel()
        {
            isFadingToLevel = true;
        }

        private void ChangeInteractability(bool boolean)
        {
            settingsButton.interactable = boolean;
            infoButton.interactable = boolean;
            interventionButton.interactable = boolean;
        }

        public void Close()
        {
            ChangeInteractability(true);
            gameObject.SetActive(false);
        }
    }
}
