using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class UI_TextNotice : MonoBehaviour
    {
        private Text _text;
        private float _displayTime = 1.0f;
        private float _displayTimer;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            _displayTimer -= Time.deltaTime;

            if (_displayTimer < 0.0f)
            {
                gameObject.SetActive(false);
            }
        }

        public void Setup(string sentence)
        {
            _displayTimer = _displayTime;
            gameObject.SetActive(true);
            _text.text = sentence;
            transform.SetAsLastSibling();
        }
    }
}

