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
    

    public void Upgrade<T>()
        where T : IUpgrade<TowerType>
    {
        bool upgradeDone = false;

        for (int i = 0; i < SpawnManager.instance.towersInField.Keys.Count; i++)
        {
            for (int j = 0; j < SpawnManager.instance.towersInField[i].Count; j++)
            {
                if (SpawnManager.instance.towersInField[i][j] is not T tower)
                {
                    continue;
                }

                if (!upgradeDone)
                {
                    switch (tower.towerType)
                    {
                        case TowerType.None:
                            break;
                        case TowerType.Diamond:
                            diamondUpgrade++;
                            break;
                        case TowerType.Hexagon:
                            hexagonUpgrade++;
                            break;
                        case TowerType.Triangle:
                            triangleUpgrade++;
                            break;
                        default:
                            break;
                    }

                    upgradeDone = true;
                }

                tower.Upgrade();
                Debug.Log($"upgraded {tower}");
            }
        }
    }
}