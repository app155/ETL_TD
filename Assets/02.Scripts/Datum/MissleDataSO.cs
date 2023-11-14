using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissleDataSO", menuName = "Scriptable Object/MissleData", order = 2)]
public class MissleDataSO : ScriptableObject
{
    public int id;
    public float moveSpeed;
    public float damageRange;
    public LayerMask targetLayer;
    public Sprite sprite;
    public Color color;
}
