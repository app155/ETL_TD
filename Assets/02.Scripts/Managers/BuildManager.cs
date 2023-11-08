using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuildManager
{
    public static BuildManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BuildManager();
            }

            return _instance;
        }
    }

    private static BuildManager _instance;
    private Dictionary<int, List<TowerController>> towersInField = new Dictionary<int, List<TowerController>>();

    public void SpawnWall(MapManager.TileInfo selectedTile)
    {
        Wall wall = PoolManager.instance.Get((int)PoolTag.Wall).GetComponent<Wall>();
        wall.tileBelong = selectedTile;
        wall.transform.position = wall.tileBelong.tilePos;
        wall.tileBelong.tileState = MapManager.TileState.Wall;
        wall.tileBelong.wall = wall.GetComponent<Wall>();
        GameManager.instance.gold -= 5;
    }

    public void SpawnDefaultTower(MapManager.TileInfo selectedTile)
    {
        if (selectedTile.tileState != MapManager.TileState.Wall)
            return;

        TowerController tower = PoolManager.instance.Get((int)PoolTag.Tower).GetComponent<TowerController>();
        tower.tileBelong = selectedTile;
        tower.gameObject.transform.position = tower.tileBelong.tilePos;
        tower.tileBelong.tileState = MapManager.TileState.Tower;
        tower.tileBelong.tower = tower;

        if (!towersInField.ContainsKey(tower.id))
        {
            towersInField[tower.id] = new List<TowerController>();
        }

        towersInField[tower.id].Add(tower);
        
        GameManager.instance.gold -= 5;
    }

    public void SpawnMissle(TowerController owner)
    {
        MissleController missle = PoolManager.instance.Get((int)PoolTag.Missle).GetComponent<MissleController>();
        missle._owner = owner;
        missle.SetUp();
    }

    public void MergeTower(TowerController selectedTower)
    {
        if (towersInField[selectedTower.id].Count <= 1)
        {
            Debug.Log("같은타워가업읍니다^^");
            return;
        }

        TowerController mergedTower = null;

        for (int i = 0; i < towersInField[selectedTower.id].Count; i++)
        {
            if (towersInField[selectedTower.id][i] == selectedTower)
                continue;

            mergedTower = towersInField[selectedTower.id][i];
            towersInField[selectedTower.id].Remove(mergedTower);
            mergedTower.tileBelong.tileState = MapManager.TileState.Wall;
            towersInField[selectedTower.id].Remove(selectedTower);
            break;
        }

        GameManager.instance.gold -= 5;
        mergedTower.gameObject.SetActive(false);

        // 선택 타워 업
    }

    public void DestroyObject(MapManager.TileInfo selectedTile)
    {
        switch ((int)selectedTile.tileState)
        {
            case 0:
                return;
            case 1:
                selectedTile.wall.gameObject.SetActive(false);
                break;
            case 2:
                selectedTile.tower.gameObject.SetActive(false);
                break;
            default:
                Console.WriteLine("DestroyObject invalid Input ㅁㄴㅇㄹㄹㄴㅁㄻㄴㅇㄻㄴㄻ");
                break;
        }
        selectedTile.tileState -= 1;
    }
}
