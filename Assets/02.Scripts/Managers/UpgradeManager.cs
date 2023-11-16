using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Controller;

public class UpgradeManager : ISceneListener
{
    public static UpgradeManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UpgradeManager();
                SceneManagerWrapped.instance.Register(_instance);
            }

            return _instance;
        }
    }

    private static UpgradeManager _instance;

    public int diamondUpgrade;
    public int hexagonUpgrade;
    public int triangleUpgrade;

    public event Action onUpgradeDone;

    //public void Upgrade<T>()
    //    where T : IUpgrade<TowerType>
    //{
    //    var type = typeof(T);



    //    for (int i = 0; i < SpawnManager.instance.towersInField.Keys.Count; i++)
    //    {
    //        for (int j = 0; j < SpawnManager.instance.towersInField[i].Count; j++)
    //        {
    //            if (SpawnManager.instance.towersInField[i][j] is not T tower)
    //            {
    //                continue;
    //            }

    //            tower.Upgrade();
    //            onUpgradeDone?.Invoke();
    //            Debug.Log($"upgraded {tower}");
    //        }
    //    }
    //}

    public void UpgradeDiamondTower()
    {
        if (GameManager.instance.gold < (diamondUpgrade + 1) * 5)
            return;

        foreach (int key in SpawnManager.instance.towersInField.Keys)
        {
            for (int i = 0; i < SpawnManager.instance.towersInField[key].Count; i++)
            {
                TowerController tower = SpawnManager.instance.towersInField[key][i];

                if (tower is DiamondTowerController)
                {
                    tower.Upgrade();
                }
            }
        }

        GameManager.instance.gold -= (diamondUpgrade + 1) * 5;
        diamondUpgrade++;
        onUpgradeDone?.Invoke();
        Debug.Log($"upgrade diamond");
    }

    public void UpgradeHexagonTower()
    {
        if (GameManager.instance.gold < (hexagonUpgrade + 1) * 5)
            return;

        foreach (int key in SpawnManager.instance.towersInField.Keys)
        {
            for (int i = 0; i < SpawnManager.instance.towersInField[key].Count; i++)
            {
                TowerController tower = SpawnManager.instance.towersInField[key][i];

                if (tower is HexagonTowerController)
                {
                    tower.Upgrade();
                }
            }
        }

        GameManager.instance.gold -= (hexagonUpgrade + 1) * 5;
        hexagonUpgrade++;
        onUpgradeDone?.Invoke();
        Debug.Log($"upgrade hexagon");
    }

    public void UpgradeTriangleTower()
    {
        if (GameManager.instance.gold < (triangleUpgrade + 1) * 5)
            return;

        foreach (int key in SpawnManager.instance.towersInField.Keys)
        {
            for (int i = 0; i < SpawnManager.instance.towersInField[key].Count; i++)
            {
                TowerController tower = SpawnManager.instance.towersInField[key][i];

                if (tower is TriangleTowerController)
                {
                    tower.Upgrade();
                }
            }
        }

        GameManager.instance.gold -= (triangleUpgrade + 1) * 5;
        triangleUpgrade++;
        onUpgradeDone?.Invoke();
        Debug.Log($"upgrade triangle");
    }

    public void OnBeforeSceneLoaded()
    {
        diamondUpgrade = 0;
        hexagonUpgrade = 0;
        triangleUpgrade = 0;
        onUpgradeDone = null;
    }

    public void OnAfterSceneLoaded()
    {
        
    }
}