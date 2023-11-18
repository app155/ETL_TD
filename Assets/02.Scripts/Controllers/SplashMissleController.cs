using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Datum;

namespace TD.Controller
{
    public class SplashMissleController : MissleController
    {
        [SerializeField] private float _damageRange;
        private GameObject _splashRangeEffect;
        private bool _isFired;

        public override void SetUp()
        {
            base.SetUp();
            _damageRange = MissleData.instance.missleDataList[owner.id].damageRange;
            _splashRangeEffect = transform.GetChild(0).gameObject;
            _splashRangeEffect.SetActive(false);
            _splashRangeEffect.transform.localScale = Vector3.one * _damageRange;
            _isFired = false;
        }

        protected override void Follow(Vector3 targetPos)
        {
            if (_isFired)
            {
                return;
            }

            base.Follow(targetPos);
        }

        protected override void Fire()
        {
            _isFired = true;

            _spriteRenderer.color = Color.clear;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _damageRange / 2.0f, Vector2.zero, 0.0f, _targetLayer);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.activeSelf == false)
                {
                    continue;
                }

                hit.collider.gameObject.GetComponent<EnemyController>().DepleteHp(_atk);
            }

            _splashRangeEffect.SetActive(true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, _damageRange / 2.0f);
        }
    }
}

