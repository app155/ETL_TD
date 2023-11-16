using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TD.UI
{
    public class UI_LifeText : MonoBehaviour
    {
        [SerializeField] private Text _lifeText;

        private void Awake()
        {
            _lifeText = GetComponent<Text>();
            GameManager.instance.onLifeDepleted += ()
                => _lifeText.text = GameManager.instance.life.ToString();
        }
    }
}

