using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashMissleController : MissleController
{
    [SerializeField] private float _damageRange;
    [SerializeField] private LayerMask _targetLayer => _owner._target.gameObject.layer;

    public override void SetUp()
    {
        base.SetUp();
        // ���������� �����Ϳ��� �ޱ�
    }

    protected override void Fire()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _damageRange, Vector2.zero, 0.0f, _targetLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.activeSelf == false)
            {
                continue;
            }

            hit.collider.gameObject.GetComponent<EnemyController>().DepleteHp(_atk);
        }

        base.Fire();
    }
}
