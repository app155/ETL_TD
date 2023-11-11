using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleTowerController : TowerController
{
    public override void SetUp(int randomNum)
    {
        base.SetUp(randomNum);

        _towerType = TowerType.Triangle;
        _upgrade = UpgradeManager.instance.triangleUpgrade;
    }
}
