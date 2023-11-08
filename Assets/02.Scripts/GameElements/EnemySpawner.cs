using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _enemySpawnCountMax;
    [SerializeField] private int _enemySpawnCount;
    [SerializeField] private float _enemySpawnTimer;
    [SerializeField] private float _enemySpawnTime;

    void Start()
    {

    }

    void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (_enemySpawnCount >= _enemySpawnCountMax)
        {
            return;
        }

        if (_enemySpawnTime < _enemySpawnTimer)
        {
            _enemySpawnTime += Time.deltaTime;
            return;
        }

        _enemySpawnCount++;
        _enemySpawnTime = 0.0f;
        PoolManager.instance.Get((int)PoolTag.Enemy);
    }
}
