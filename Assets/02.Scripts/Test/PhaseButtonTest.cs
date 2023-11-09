using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseButtonTest : MonoBehaviour
{
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            GameManager.instance.gamePhase = GamePhase.DefensePhase;
            StartCoroutine(SpawnManager.instance.SpawnRoundEnemy());
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
