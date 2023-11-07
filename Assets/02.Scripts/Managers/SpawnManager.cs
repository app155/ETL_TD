using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void SpawnWall()
    {
        PoolManager.instance.Get((int)PoolTag.Wall);
    }

    void SpawnEnemy()
    {
        PoolManager.instance.Get((int)PoolTag.Enemy);
    }
}
