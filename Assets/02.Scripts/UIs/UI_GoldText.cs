using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    public class UI_GoldText : MonoBehaviour
    {
        [SerializeField] private Text _goldText;

        private void Awake()
        {
            _goldText = GetComponent<Text>();
            GameManager.instance.onGoldChanged += ()
                => _goldText.text = GameManager.instance.gold.ToString();
        }
    }
}
