using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Controller;

public class PoolOfTower : GameObjectPool
{
    protected override void OnReturnToPool(GameObject item)
    {
        base.OnReturnToPool(item);

        foreach (var controller in item.GetComponents<TowerController>())
        {
            controller.enabled = false;
        }

        // ¤Ð¤Ð
        //Destroy(item.GetComponent<TowerController>());
    }
}
