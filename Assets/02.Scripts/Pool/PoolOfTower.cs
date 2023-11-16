using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Controller;

public class PoolOfTower : GameObjectPool
{
    protected override void OnReturnToPool(GameObject item)
    {
        base.OnReturnToPool(item);

        // ¤Ð¤Ð
        Destroy(item.GetComponent<TowerController>());
    }
}
