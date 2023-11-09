using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextNotificator : MonoBehaviour
{
    [SerializeField] private Text _textNotificator;
    private Color _originColor;

    void Awake()
    {
        _textNotificator = GetComponentInChildren<Text>();
        _originColor = _textNotificator.color;
    }

    private void OnEnable()
    {
        _textNotificator.color = _originColor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * 0.05f * Time.deltaTime;
        Color color = Color.black;
    }
}
