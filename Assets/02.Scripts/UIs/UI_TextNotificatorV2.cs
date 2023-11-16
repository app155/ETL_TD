using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextNotificatorV2 : MonoBehaviour
{
    
    [SerializeField] private Text[] _texts;
    private Queue<Text> _textQueue;
    [SerializeField] private float _displayTime;

    private void Awake()
    {
        _textQueue = new Queue<Text>(GetComponentsInChildren<Text>());

        GameManager.instance.onTextNotifyRequired += (sentence) =>
        {
            StartCoroutine(DisplayRoutine(_textQueue.Dequeue(), sentence));
        };
    }

    private void Start()
    {
        
    }

    IEnumerator DisplayRoutine(Text text, string sentence)
    {
        text.text = sentence;
        text.gameObject.SetActive(true);

        _textQueue.Enqueue(text);
        yield return new WaitForSeconds(_displayTime);

        text.gameObject.SetActive(false);
    }
}
