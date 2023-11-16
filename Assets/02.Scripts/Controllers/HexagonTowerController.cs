using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Controller
{
    public class HexagonTowerController : TowerController
    {
        public override void SetUp(int randomNum)
        {
            base.SetUp(randomNum);

            _towerType = TowerType.Hexagon;
            _upgrade = UpgradeManager.instance.hexagonUpgrade;
        }
    }
}

