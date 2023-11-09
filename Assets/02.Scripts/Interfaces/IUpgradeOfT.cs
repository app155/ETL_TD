using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgrade<T>
    where T : Enum
{
    public T towerType { get; }
    public int upgrade { get; set; }
    public int upgradeMax { get; }
    public int upgradeMin { get; }
    public int upgradeGain { get; }

    public void Upgrade();
}
