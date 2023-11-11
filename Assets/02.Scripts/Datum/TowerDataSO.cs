using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataSO", menuName = "Scriptable Object/TowerData", order = 1)]
public class TowerDataSO : ScriptableObject
{
    public int id;
    public TowerType towerType;
    public AttackType attackType;
    public int level;
    public int atkBase;
    public float atkRange;
    public float atkTime;
    public int upgradeGain;
    public LayerMask targetLayer;
    public Sprite sprite;
    public Color color;
}
