using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TD.Controller;

namespace TD.UI
{
    public class MiniUI_EnemyHpBar : MonoBehaviour
    {
        [SerializeField] Slider _hpBar;
        EnemyController _enemy;

        private void Awake()
        {
            _enemy = transform.root.GetComponent<EnemyController>();
            _hpBar = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            _hpBar.maxValue = _enemy.hpMax;
            _hpBar.minValue = 0;
            _hpBar.value = _enemy.hp;
        }

        void Start()
        {
            _hpBar.maxValue = _enemy.hpMax;
            _hpBar.minValue = 0;
            _hpBar.value = _enemy.hp;

            _enemy.onHpChanged += () => _hpBar.value = _enemy.hp;
        }
    }
}

