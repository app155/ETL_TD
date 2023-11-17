using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TD.Controller;
using TD.Datum;
using Random = UnityEngine.Random;

public class SpawnManager : ISceneListener
{
    public static SpawnManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SpawnManager();
                SceneManagerWrapped.instance.Register(_instance);
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

    public void SpawnWall(TileInfo selectedTile)
    {   
        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            GameManager.instance.TextNotify("Cannot spawn wall during Wave");
            return;
        }

        if (GameManager.instance.gold < 5)
        {
            GameManager.instance.TextNotify("Not enough Gold");
            return;
        }

        if (MapManager.instance.PathFindbyBFS(selectedTile, MapManager.instance.map) == false)
        {
            GameManager.instance.TextNotify("Cannot spawn wall in tile where blocks the path");
            return;
        }

        Wall wall = PoolManager.instance.Get((int)PoolTag.Wall).GetComponent<Wall>();
        wall.tileBelong = selectedTile;
        wall.transform.position = wall.tileBelong.tilePos;
        wall.tileBelong.tileState = TileState.Wall;
        wall.tileBelong.wall = wall.GetComponent<Wall>();

        GameManager.instance.gold -= 5;

        MapManager.instance.onSelectedTileChanged?.Invoke();
    }

    public void SpawnDefaultTower(TileInfo selectedTile)
    {
        if (selectedTile.tileState != TileState.Wall)
        {
            GameManager.instance.TextNotify("Tower can be spawned in tile where wall created");
            return;
        }

        if (GameManager.instance.gold < 5)
        {
            GameManager.instance.TextNotify("Not enough Gold");
            return;
        }

        int randomNum = Random.Range(0, 3);

        TowerController tower;

        switch (randomNum)
        {
            case 0:
                tower = PoolManager.instance.Get((int)PoolTag.Tower).GetComponent<DiamondTowerController>();
                break;
            case 1:
                tower = PoolManager.instance.Get((int)PoolTag.Tower).GetComponent<HexagonTowerController>();
                break;
            default:
                tower = PoolManager.instance.Get((int)PoolTag.Tower).GetComponent<TriangleTowerController>();
                break;
        }

        tower.enabled = true;
        tower.tileBelong = selectedTile;
        tower.gameObject.transform.position = tower.tileBelong.tilePos;
        tower.tileBelong.tileState = TileState.Tower;
        tower.tileBelong.tower = tower;

        tower.SetUp(randomNum);

        if (!towersInField.ContainsKey(tower.id))
        {
            towersInField[tower.id] = new List<TowerController>();
        }

        towersInField[tower.id].Add(tower);

        onTowerSpawned?.Invoke();
        
        GameManager.instance.gold -= 5;

        MapManager.instance.onSelectedTileChanged?.Invoke();
    }

    public void SpawnMissle(TowerController owner)
    {
        MissleController missle;

        switch (owner.attackType)
        {
            case AttackType.None:
                return;
            case AttackType.Normal:
                missle = PoolManager.instance.Get((int)PoolTag.Missle).GetComponent<NormalMissleController>();
                break;
            case AttackType.Splash:
                missle = PoolManager.instance.Get((int)PoolTag.Missle).GetComponent<SplashMissleController>();
                break;
            default:
                return;
        };

        missle.enabled = true;
        missle.owner = owner;
        missle.SetUp();
    }

    public void MergeTower(TowerController selectedTower)
    {
        if (GameManager.instance.gold < 5)
        {
            GameManager.instance.TextNotify("Not enough Gold");
            return;
        }

        if (towersInField[selectedTower.id].Count <= 1)
        {
            GameManager.instance.TextNotify("The same tower does not exist on the field");
            return;
        }

        if (3 * (selectedTower.level + 1) >= TowerData.instance.towerDataList.Count)
        {
            GameManager.instance.TextNotify("The tower has already reached maximum level");
            return;
        }

        TowerController mergedTower = null;
        TileInfo tile = selectedTower.tileBelong;

        for (int i = 0; i < towersInField[selectedTower.id].Count; i++)
        {
            if (towersInField[selectedTower.id][i] == selectedTower)
                continue;

            mergedTower = towersInField[selectedTower.id][i];
            towersInField[selectedTower.id].Remove(mergedTower);
            mergedTower.tileBelong.tileState = TileState.Wall;
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

        MapManager.instance.onSelectedTileChanged?.Invoke();
    }

    public void DestroyObject(TileInfo selectedTile)
    {
        switch ((int)selectedTile.tileState)
        {
            case 0:
                return;
            case 1:
                if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
                {
                    GameManager.instance.TextNotify("Cannot destroy wall During Wave");
                    return;
                }

                selectedTile.wall.gameObject.SetActive(false);
                break;
            case 2:
                selectedTile.tower.gameObject.SetActive(false);
                onTowerDestroyed?.Invoke();
                break;
            default:
                Console.WriteLine("DestroyObject invalid Input");
                break;
        }

        GameManager.instance.gold -= 5;
        selectedTile.tileState -= 1;

        MapManager.instance.onSelectedTileChanged?.Invoke();
    }

    public IEnumerator SpawnPathNotificatorRoutine()
    {
        if (_canNotifyPath == false)
        {
            GameManager.instance.TextNotify("Path preview is already running");
            yield break;
        }

        if (GameManager.instance.gamePhase != GamePhase.BuildPhase)
        {
            GameManager.instance.TextNotify("Path preview is only available in buildphase");
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

    public void OnBeforeSceneLoaded()
    {
        onDefensePhaseStarted = null;
        onTowerSpawned = null;
        onTowerDestroyed = null;
        _enemySpawnCount = 0;
        towersInField.Clear();
    }

    public void OnAfterSceneLoaded()
    {
        //SceneManagerWrapped.instance.Register(_instance);
    }
}
