using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TD.Datum;

namespace TD.UI
{
    public class UI_CurrentRoundEnemyInfoDisplayer : MonoBehaviour
    {
        [SerializeField] private Image _currentEnemyImage;
        [SerializeField] private Text _currentEnemyHpText;

        void Start()
        {
            _currentEnemyImage.sprite = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].sprite;
            _currentEnemyHpText.text = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].hpMax.ToString();

            GameManager.instance.onDefencePhaseEnded += () =>
            {
                if (GameManager.instance.gamePhase == GamePhase.GameClear)
                {
                    _currentEnemyImage.sprite = null;
                    _currentEnemyHpText.text = "";
                }

                else
                {
                    _currentEnemyImage.sprite = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].sprite;
                    _currentEnemyHpText.text = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].hpMax.ToString();
                }
            };
        }
    }
}


