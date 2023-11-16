using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PhaseButton : MonoBehaviour
{
    Button _button;
    Text _text;

    // Start is called before the first frame update
    void Awake()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        _text.text = "Round 1\nStart";

        _button.onClick.AddListener(() =>
        {
            GameManager.instance.gamePhase = GamePhase.DefensePhase;
            _button.interactable = false;
            StartCoroutine(SpawnManager.instance.SpawnRoundEnemyRoutine());
        });

        GameManager.instance.onDefencePhaseEnded += () =>
        {
            _button.interactable = true;
            _text.text = $"Round {GameManager.instance.round + 1}\nStart";
        };
    }
}
