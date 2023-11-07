using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager
{
    public static SpawnManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SpawnManager();
            }

            return _instance;
        }
    }

    private static SpawnManager _instance;
    private Dictionary<int, List<TowerController>> towersInField = new Dictionary<int, List<TowerController>>();

    public void SpawnWall(int y, int x)
    {
        GameObject wall = PoolManager.instance.Get((int)PoolTag.Wall);
        wall.transform.position = MapManager.instance._map[y, x].tilePos;
        MapManager.instance._map[y, x].tileState = MapManager.TileState.Wall;
    }

    public void SpawnEnemy()
    {
        PoolManager.instance.Get((int)PoolTag.Enemy);
    }

    public void SpawnDefaultTower()
    {
        TowerController tower = PoolManager.instance.Get((int)PoolTag.Tower).GetComponent<TowerController>();
    }
}
