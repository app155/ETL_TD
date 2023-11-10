using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseButtonTest : MonoBehaviour
{
    Button button;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            GameManager.instance.gamePhase = GamePhase.DefensePhase;
            button.interactable = false;
            StartCoroutine(SpawnManager.instance.SpawnRoundEnemy());
        });
        GameManager.instance.onDefencePhaseEnded += () => button.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
