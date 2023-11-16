using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Controller;

public class PoolOfTower : GameObjectPool
{
    protected override void OnReturnToPool(GameObject item)
    {
        base.OnReturnToPool(item);

        // �Ф�
        Destroy(item.GetComponent<TowerController>());
    }
}
