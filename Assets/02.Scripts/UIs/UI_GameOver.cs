using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameEnd : MonoBehaviour
{
    [SerializeField] private Text _endText;
    [SerializeField] private Text _resultText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    void Start()
    {
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

        _restartButton.onClick.AddListener(() =>
        {
            SceneManagerWrapped.instance.LoadScene(0);
            Time.timeScale = 1.0f;
        });

        _quitButton.onClick.AddListener(() => Application.Quit());

        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
