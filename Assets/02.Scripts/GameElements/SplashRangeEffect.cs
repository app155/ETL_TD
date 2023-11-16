using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Controller;

namespace TD.GameElements
{
    public class SplashRangeEffect : MonoBehaviour
    {
        private MissleController _owner;
        [SerializeField] private float _range;
        [SerializeField] private float _effectTime = 0.1f;
        [SerializeField] private float _effectTimer;

        private void Awake()
        {
            _owner = transform.root.GetComponent<MissleController>();
        }

        private void Update()
        {
            if (_effectTimer <= _effectTime)
            {
                _effectTimer += Time.deltaTime;
            }

            else
            {
                _effectTimer = 0.0f;
                gameObject.SetActive(false);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
