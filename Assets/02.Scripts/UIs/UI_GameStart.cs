using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class UI_GameStart : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Image _moveImage;

        private float _moveDir;

        private void Awake()
        {
            _startButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                GameManager.instance.gamePhase = GamePhase.BuildPhase;
            });
            _quitButton.onClick.AddListener(() => Application.Quit());

        }

        private void Update()
        {
            _moveDir += Time.deltaTime;

            _moveImage.rectTransform.anchoredPosition += Vector2.up * Mathf.Cos(_moveDir) * 0.2f;
        }
    }
}
