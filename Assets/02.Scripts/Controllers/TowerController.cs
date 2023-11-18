using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Datum;

namespace TD.Controller
{
    public enum TowerType
    {
        //temp?
        None,
        Diamond,
        Hexagon,
        Triangle,
    }

    public enum AttackType
    {
        None,
        Normal,
        Splash,
    }

    public abstract class TowerController : MonoBehaviour, IUpgrade<TowerType>
    {
        public TowerType towerType => _towerType;

        [SerializeField] protected TowerType _towerType;
        public AttackType attackType;

        public int id;
        public int level;
        public TileInfo tileBelong;

        public int atk => _atkBase + _upgrade * _upgradeGain;
        [SerializeField] protected int _atkBase;
        [SerializeField] protected float _atkRange;
        [SerializeField] protected float _atkTime;
        [SerializeField] protected float _atkTimer;

        public int upgrade
        {
            get { return _upgrade; }
            set
            {
                _upgrade = Mathf.Clamp(value, upgradeMin, upgradeMax);
            }
        }

        public int upgradeMax { get => _upgradeMax; }
        public int upgradeMin { get => _upgradeMin; }
        public int upgradeGain { get => _upgradeGain; }

        [SerializeField] protected int _upgrade;
        [SerializeField] protected int _upgradeMax = 255;
        [SerializeField] protected int _upgradeMin = 0;
        [SerializeField] protected int _upgradeGain;

        public Transform _target;
        [SerializeField] protected MissleController _missle;
        [SerializeField] protected LayerMask _targetLayer;
        protected SpriteRenderer _spriteRenderer;
        protected Color _spriteColor;

        protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Start()
        {

        }

        protected void Update()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (_target == null)
            {
                _target = Search();
            }

            LaunchMissle();
        }

        public void Upgrade()
        {
            upgrade++;
        }

        protected Transform Search()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _atkRange, Vector2.zero, 0.0f, _targetLayer);

            if (hits.Length > 0)
            {
                Transform target = hits[0].transform;

                for (int i = 1; i < hits.Length; i++)
                {
                    if (Vector2.Distance(transform.position, target.position) > Vector2.Distance(transform.position, hits[i].transform.position))
                    {
                        target = hits[i].transform;
                    }
                }

                return target;
            }

            return null;
        }

        protected void LaunchMissle()
        {
            _atkTimer -= Time.fixedDeltaTime;

            if (_target == null)
            {
                return;
            }

            if (_target.gameObject.activeSelf == false || Vector2.Distance(transform.position, _target.position) > _atkRange)
            {
                _target = null;
                return;
            }

            if (_atkTimer <= 0.0f)
            {
                SpawnManager.instance.SpawnMissle(this);
                _atkTimer = _atkTime;
            }
        }

        //adsfsafdsafdsaf
        public virtual void SetUp(int randomNum)
        {
            id = TowerData.instance.towerDataList[randomNum].id;
            attackType = TowerData.instance.towerDataList[randomNum].attackType;
            level = TowerData.instance.towerDataList[randomNum].level;
            _atkBase = TowerData.instance.towerDataList[randomNum].atkBase;
            _atkRange = TowerData.instance.towerDataList[randomNum].atkRange;
            _atkTime = TowerData.instance.towerDataList[randomNum].atkTime;
            _upgradeGain = TowerData.instance.towerDataList[randomNum].upgradeGain;
            _spriteRenderer.sprite = TowerData.instance.towerDataList[randomNum].sprite;
            _spriteRenderer.color = TowerData.instance.towerDataList[randomNum].color;
            _targetLayer = TowerData.instance.towerDataList[randomNum].targetLayer;
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _atkRange);
        }
    }
}
