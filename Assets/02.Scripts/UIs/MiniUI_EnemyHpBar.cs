using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniUI_EnemyHpBar : MonoBehaviour
{
    [SerializeField] Slider _hpBar;

    private void Awake()
    {
        _hpBar = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        EnemyController enemy = transform.root.GetComponent<EnemyController>();

        _hpBar.maxValue = enemy.hpMax;
        _hpBar.minValue = 0;
        _hpBar.value = enemy.hp;

        enemy.onHpChanged += () => _hpBar.value = enemy.hp;
    }
}
