using System;
using System.Collections;
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

    public int enemyRemainCount
    {
        get
        {
            return _enemyRemainCount;
        }

        set
        {
            _enemyRemainCount = value;

            if (_enemyRemainCount <= 0)
            {
                GameManager.instance.gamePhase = GamePhase.BuildPhase;
                GameManager.instance.round++;
                Debug.Log($"gamephase to buildphase & Round Advance {GameManager.instance.round}");
            }
        }
    }

    public Action onDefensePhaseStarted;

    private int _enemyRemainCount;
    private int _enemySpawnCountMax = 10;
    private int _enemySpawnCount;
    private float _enemySpawnTimer = 1.0f;

    private static SpawnManager _instance;
    private Dictionary<int, List<TowerController>> towersInField = new Dictionary<int, List<TowerController>>();

    public void SpawnWall(MapManager.TileInfo selectedTile)
    {
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            Debug.Log("웨이브중 벽 건설불가");
            return;
        }
            

        if (selectedTile.tileState != MapManager.TileState.Wall)
            return;

        Wall wall = PoolManager.instance.Get((int)PoolTag.Wall).GetComponent<Wall>();
        wall.tileBelong = selectedTile;
        wall.transform.position = wall.tileBelong.tilePos;
        wall.tileBelong.tileState = MapManager.TileState.Wall;
        wall.tileBelong.wall = wall.GetComponent<Wall>();
        GameManager.instance.gold -= 5;
    }

    public void SpawnDefaultTower(MapManager.TileInfo selectedTile)
    {
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            Debug.Log("웨이브중 타워 건설불가");
            return;
        }

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
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            Debug.Log("웨이브중 타워 진화불가");
            return;
        }

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
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            Debug.Log("웨이브중 해체 불가");
            return;
        }

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

    public IEnumerator SpawnPathNotificator()
    {
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            yield break;
        }

        int index = 0;

        while (index < MapManager.instance.path.Count)
        {
            GameObject item = PoolManager.instance.Get((int)PoolTag.PathNotificator);
            item.transform.position = MapManager.instance.map[MapManager.instance.path[index][0], MapManager.instance.path[index][1]].tilePos;
            index++;

            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator SpawnRoundEnemy()
    {
        if (GameManager.instance.gamePhase != GamePhase.DefensePhase)
        {
            Debug.Log("cannot spawnEnemy");
            yield break;
        }    

        _enemyRemainCount = _enemySpawnCountMax;
        yield return new WaitForSeconds(1.0f);

        while (_enemySpawnCount < _enemySpawnCountMax)
        {
            PoolManager.instance.Get((int)PoolTag.Enemy);
            _enemySpawnCount++;
            Debug.Log(_enemySpawnCount);
            yield return new WaitForSeconds(_enemySpawnTimer);
        }

        _enemySpawnCount = 0;
        Debug.Log(_enemySpawnCount);
    }
}
