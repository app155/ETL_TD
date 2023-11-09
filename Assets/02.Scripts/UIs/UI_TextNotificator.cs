using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextNotificator : MonoBehaviour
{
    [SerializeField] private Text _textNotificator;

    [SerializeField] private float _time;
    [SerializeField] private float _timer;

    void Awake()
    {
        _textNotificator = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        _timer = _time;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
