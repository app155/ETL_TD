using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Scriptable Object/EnemySpawnData", order = 0)]
public class EnemySpawnData : ScriptableObject
{
    public int id;
    public float moveSpeed;
    public Sprite sprite;
}