using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Controller;

public class PoolOfMissle : GameObjectPool
{
    protected override void OnReturnToPool(GameObject item)
    {
        base.OnReturnToPool(item);

        foreach (var controller in item.GetComponents<MissleController>())
        {
            controller.enabled = false;
        }
    }
}
