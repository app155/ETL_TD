using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashMissleController : MissleController
{
    [SerializeField] private float _damageRange;
    [SerializeField] private float _effectDisplayTime = 0.5f;
    [SerializeField] private float _effectDisplayTimer;
    private GameObject _splashRangeEffect;

    public override void SetUp()
    {
        base.SetUp();
        _damageRange = MissleData.instance.missleDataList[owner.id].damageRange;
        _splashRangeEffect = transform.GetChild(0).gameObject;
    }

    protected override void Fire()
    {
        _splashRangeEffect.transform.localScale *= _damageRange;
        _splashRangeEffect.SetActive(true);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _damageRange, Vector2.zero, 0.0f, _targetLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.activeSelf == false)
            {
                continue;
            }

            hit.collider.gameObject.GetComponent<EnemyController>().DepleteHp(_atk);
        }

        while (_effectDisplayTimer <= _effectDisplayTime)
            _effectDisplayTimer += Time.deltaTime;

        _splashRangeEffect.transform.localScale = Vector3.one;
        base.Fire();
    }

}
