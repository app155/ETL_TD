using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTowerController : TowerController, IUpgrade<TowerType>
{
    public TowerType towerType { get => _towerType; }
    public int upgrade
    {
        get { return _upgrade; }
        set 
        { 
            _upgrade = Mathf.Clamp(value, upgradeMin, upgradeMax);
            onTriangleUpgraded?.Invoke();
        }
    }

    public int upgradeMax { get => _upgradeMax; }
    public int upgradeMin { get => _upgradeMin; }
    public int upgradeGain { get => _upgradeGain; }

    public event Action onTriangleUpgraded;

    public void Upgrade()
    {
        upgrade++;
    }
}
