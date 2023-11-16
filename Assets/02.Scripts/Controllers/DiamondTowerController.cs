using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Controller
{
    public class DiamondTowerController : TowerController, IUpgrade<TowerType>
    {
        public override void SetUp(int randomNum)
        {
            base.SetUp(randomNum);

            _towerType = TowerType.Diamond;
            _upgrade = UpgradeManager.instance.diamondUpgrade;
        }
    }
}
