using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class UI_TextNotificatorV2 : MonoBehaviour
    {
        [SerializeField] private UI_TextNotice[] _texts;
        private Queue<UI_TextNotice> _textQueue;
        [SerializeField] private float _displayTime;

        private void Awake()
        {
            _textQueue = new Queue<UI_TextNotice>(_texts);

            GameManager.instance.onTextNotifyRequired += (sentence) =>
            {
                DisplayText(_textQueue.Dequeue(), sentence);
            };
        }

        private void Start()
        {

        }

        void DisplayText(UI_TextNotice text, string sentence)
        {
            text.Setup(sentence);
            _textQueue.Enqueue(text);
        }
    }
}

