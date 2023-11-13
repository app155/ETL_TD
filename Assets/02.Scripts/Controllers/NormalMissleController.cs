using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMissleController : MissleController
{
    public override void SetUp()
    {
        base.SetUp();
    }

    protected override void Fire()
    {
        if (_target.gameObject.activeSelf != false)
        {
            _target.GetComponent<EnemyController>().DepleteHp(_atk);
        }

        base.Fire();
    }
}
