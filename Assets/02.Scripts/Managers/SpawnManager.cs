using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

            Debug.Log($"remaincount set! {_enemyRemainCount}");

            if (_enemyRemainCount == 0)
            {
                GameManager.instance.EndDefensePhase();
                Debug.Log($"gamephase to buildphase & Round Advance {GameManager.instance.round}");
            }
        }
    }

    public Action onDefensePhaseStarted;
    public Action onTowerSpawned;
    public Action onTowerDestroyed;

    private bool _canNotifyPath = true;

    private int _enemyRemainCount;
    private int _enemySpawnCountMax = 30;
    private int _enemySpawnCount;
    private float _enemySpawnTimer = 1.0f;

    private static SpawnManager _instance;
    public Dictionary<int, List<TowerController>> towersInField = new Dictionary<int, List<TowerController>>();

    public void SpawnWall(MapManager.TileInfo selectedTile)
    {
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            SpawnUITextNotification("웨이브 중 벽 건설이 불가능합니다.");
            return;
        }
            
        if (selectedTile.tileState != MapManager.TileState.Wall)
        {
            SpawnUITextNotification("벽은 빈 타일에만 건설 가능합니다.");
            return;
        }

        if (GameManager.instance.gold < 5)
        {
            SpawnUITextNotification("자원이 부족합니다.");
            return;
        }

        Wall wall = PoolManager.instance.Get((int)PoolTag.Wall).GetComponent<Wall>();
        wall.tileBelong = selectedTile;
        wall.transform.position = wall.tileBelong.tilePos;
        wall.tileBelong.tileState = MapManager.TileState.Wall;
        wall.tileBelong.wall = wall.GetComponent<Wall>();

        GameManager.instance.gold -= 5;
    }

    public void SpawnDefaultTower(MapManager.TileInfo selectedTile)
    {
        //if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        //{
        //    SpawnUITextNotification("웨이브중 타워 건설이 불가능합니다.");
        //    return;
        //}

        if (selectedTile.tileState != MapManager.TileState.Wall)
        {
            SpawnUITextNotification("타워는 벽이 건설된 타일에만 건설 가능합니다.");
            return;
        }

        if (GameManager.instance.gold < 5)
        {
            SpawnUITextNotification("자원이 부족합니다.");
            return;
        }

        int randomNum = Random.Range(0, 3);

        TowerController tower;

        switch (randomNum)
        {
            case 0:
                tower = PoolManager.instance.Get((int)PoolTag.Tower).AddComponent<DiamondTowerController>();
                break;
            case 1:
                tower = PoolManager.instance.Get((int)PoolTag.Tower).AddComponent<HexagonTowerController>();
                break;
            default:
                tower = PoolManager.instance.Get((int)PoolTag.Tower).AddComponent<TriangleTowerController>();
                break;
        }

        tower.tileBelong = selectedTile;
        tower.gameObject.transform.position = tower.tileBelong.tilePos;
        tower.tileBelong.tileState = MapManager.TileState.Tower;
        tower.tileBelong.tower = tower;

        tower.SetUp(randomNum);

        if (!towersInField.ContainsKey(tower.id))
        {
            towersInField[tower.id] = new List<TowerController>();
        }

        towersInField[tower.id].Add(tower);

        onTowerSpawned?.Invoke();
        
        GameManager.instance.gold -= 5;
    }

    public void SpawnMissle(TowerController owner)
    {
        MissleController missle;

        switch (owner.attackType)
        {
            case AttackType.None:
                return;
            case AttackType.Normal:
                missle = PoolManager.instance.Get((int)PoolTag.Missle).AddComponent<NormalMissleController>();
                break;
            case AttackType.Splash:
                missle = PoolManager.instance.Get((int)PoolTag.Missle).AddComponent<SplashMissleController>();
                break;
            default:
                return;
        };

        missle.owner = owner;
        missle.SetUp();
    }

    public void MergeTower(TowerController selectedTower)
    {
        //if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        //{
        //    SpawnUITextNotification("웨이브 중 타워를 진화시킬 수 없습니다.");
        //    return;
        //}

        if (towersInField[selectedTower.id].Count <= 1)
        {
            SpawnUITextNotification("필드에 선택한 타워와 같은 종류의 타워가 없습니다.");
            return;
        }

        if (3 * (selectedTower.level + 1) >= TowerData.instance.towerDataList.Count)
        {
            SpawnUITextNotification("최대 레벨에 도달한 타워입니다.");
            return;
        }

        TowerController mergedTower = null;
        MapManager.TileInfo tile = selectedTower.tileBelong;

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
        mergedTower.tileBelong.tower = null;
        mergedTower.gameObject.SetActive(false);

        int randomNum = Random.Range(3 * (selectedTower.level + 1), 3 * (selectedTower.level + 2));
        selectedTower.gameObject.SetActive(false);

        switch (randomNum % 3)
        {
            case 0:
                selectedTower = PoolManager.instance.Get((int)PoolTag.Tower).AddComponent<DiamondTowerController>();
                break;
            case 1:
                selectedTower = PoolManager.instance.Get((int)PoolTag.Tower).AddComponent<HexagonTowerController>();
                break;
            default:
                selectedTower = PoolManager.instance.Get((int)PoolTag.Tower).AddComponent<TriangleTowerController>();
                break;
        }

        selectedTower.tileBelong = tile;
        tile.tower = selectedTower;
        selectedTower.SetUp(randomNum);

        if (!towersInField.ContainsKey(selectedTower.id))
        {
            towersInField[selectedTower.id] = new List<TowerController>();
        }

        onTowerSpawned?.Invoke();

        towersInField[selectedTower.id].Add(selectedTower);
    }

    public void DestroyObject(MapManager.TileInfo selectedTile)
    {
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            SpawnUITextNotification("벽과 타워는 웨이브 중 파괴할 수 없습니다.");
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
                onTowerDestroyed?.Invoke();
                break;
            default:
                Console.WriteLine("DestroyObject invalid Input ㅁㄴㅇㄹㄹㄴㅁㄻㄴㅇㄻㄴㄻ");
                break;
        }
        selectedTile.tileState -= 1;
    }

    public IEnumerator SpawnPathNotificatorRoutine()
    {
        if (_canNotifyPath == false)
        {
            SpawnUITextNotification("경로 표시가 이미 진행중입니다.");
            yield break;
        }

        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            SpawnUITextNotification("경로 확인은 준비 단계에서만 가능합니다.");
            yield break;
        }

        int index = 0;
        _canNotifyPath = false;

        while (index < MapManager.instance.path.Count)
        {
            GameObject item = PoolManager.instance.Get((int)PoolTag.PathNotificator);
            item.transform.position = MapManager.instance.map[MapManager.instance.path[index][0], MapManager.instance.path[index][1]].tilePos;
            index++;

            yield return new WaitForSeconds(0.1f);
        }

        _canNotifyPath = true;
    }

    public IEnumerator SpawnRoundEnemyRoutine()
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
            yield return new WaitForSeconds(_enemySpawnTimer);
        }

        Debug.Log("Finish EnemySpawn");
        _enemySpawnCount = 0;
    }

    public void SpawnUITextNotification(string message)
    {
        Text text = PoolManager.instance.Get((int)PoolTag.UI_TextNotification).GetComponentInChildren<Text>();
        text.text = message;
    }
}
