using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolOfEnemy : GameObjectPool
{
    protected override void OnGetFromPool(GameObject item)
    {
        base.OnGetFromPool(item);
    }
}
