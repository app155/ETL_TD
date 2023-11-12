using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.onGameOver += () =>
        {
            gameObject.SetActive(true);
        };

        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
