using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TD.UI
{
    public class UI_GameEnd : MonoBehaviour
    {
        [SerializeField] private Text _endText;
        [SerializeField] private Text _resultText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _quitButton;

        void Start()
        {
            GameManager.instance.onGamePaused += () =>
            {
                gameObject.SetActive(true);
                _endText.text = "Paused";
                _resultText.text = $"Wave {GameManager.instance.round + 1}";
                _continueButton.gameObject.SetActive(true);
            };

            GameManager.instance.onGameOver += () =>
            {
                gameObject.SetActive(true);
                _endText.text = "Game Over...";
                _resultText.text = $"Wave {GameManager.instance.round + 1}";
            };

            GameManager.instance.onGameClear += () =>
            {
                gameObject.SetActive(true);
                _endText.text = "Clear!";
                _resultText.text = "";
            };

            _continueButton.onClick.AddListener(() =>
            {
                GameManager.instance.EndGamePause();
                gameObject.SetActive(false);
            });

            _restartButton.onClick.AddListener(() =>
            {
                SceneManagerWrapped.instance.LoadScene(0);
            });

            _quitButton.onClick.AddListener(() => Application.Quit());

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _continueButton.gameObject.SetActive(false);
        }
    }
}
