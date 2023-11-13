using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolOfMissle : GameObjectPool
{
    protected override void OnReturnToPool(GameObject item)
    {
        base.OnReturnToPool(item);

        // ¤Ð¤Ð
        Destroy(item.GetComponent<MissleController>());
    }
}
