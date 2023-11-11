using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager
{
    public static UpgradeManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UpgradeManager();
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
    //    bool upgradeDone = false;

    //    for (int i = 0; i < SpawnManager.instance.towersInField.Keys.Count; i++)
    //    {
    //        for (int j = 0; j < SpawnManager.instance.towersInField[i].Count; j++)
    //        {
    //            if (SpawnManager.instance.towersInField[i][j] is not T tower)
    //            {
    //                continue;
    //            }

    //            if (!upgradeDone)
    //            {
    //                if (GameManager.instance.gold < (tower.upgrade + 1) * 5)
    //                    return;

    //                switch (tower.towerType)
    //                {
    //                    case TowerType.None:
    //                        break;
    //                    case TowerType.Diamond:
    //                        diamondUpgrade++;
    //                        break;
    //                    case TowerType.Hexagon:
    //                        hexagonUpgrade++;
    //                        break;
    //                    case TowerType.Triangle:
    //                        triangleUpgrade++;
    //                        break;
    //                    default:
    //                        break;
    //                }

    //                upgradeDone = true;
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

        for (int i = 0; i < SpawnManager.instance.towersInField.Keys.Count; i++)
        {
            for (int j = 0; j < SpawnManager.instance.towersInField[i].Count; j++)
            {
                TowerController tower = SpawnManager.instance.towersInField[i][j];

                if (tower is DiamondTowerController)
                {
                    tower.Upgrade();
                }
            }
        }

        GameManager.instance.gold -= (diamondUpgrade + 1) * 5;
        diamondUpgrade++;
        onUpgradeDone?.Invoke();
        Debug.Log($"upgraded diamond");
    }

    public void UpgradeHexagonTower()
    {
        if (GameManager.instance.gold < (hexagonUpgrade + 1) * 5)
            return;

        for (int i = 0; i < SpawnManager.instance.towersInField.Keys.Count; i++)
        {
            for (int j = 0; j < SpawnManager.instance.towersInField[i].Count; j++)
            {
                TowerController tower = SpawnManager.instance.towersInField[i][j];

                if (tower is HexagonTowerController)
                {
                    tower.Upgrade();
                }
            }
        }

        GameManager.instance.gold -= (hexagonUpgrade + 1) * 5;
        hexagonUpgrade++;
        onUpgradeDone?.Invoke();
        Debug.Log($"upgraded hexagon");
    }

    public void UpgradeTriangleTower()
    {
        if (GameManager.instance.gold < (triangleUpgrade + 1) * 5)
            return;

        for (int i = 0; i < SpawnManager.instance.towersInField.Keys.Count; i++)
        {
            for (int j = 0; j < SpawnManager.instance.towersInField[i].Count; j++)
            {
                TowerController tower = SpawnManager.instance.towersInField[i][j];

                if (tower is TriangleTowerController)
                {
                    tower.Upgrade();
                }
            }
        }

        GameManager.instance.gold -= (triangleUpgrade + 1) * 5;
        triangleUpgrade++;
        onUpgradeDone?.Invoke();
        Debug.Log($"upgraded triangle");
    }
}