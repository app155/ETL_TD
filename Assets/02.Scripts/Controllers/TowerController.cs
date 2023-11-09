using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public abstract class TowerController : MonoBehaviour
{
    [SerializeField] protected TowerType _towerType;
    public AttackType _attackType;

    public int id;
    public int level;
    public MapManager.TileInfo tileBelong;

    public int _atk;
    [SerializeField] protected int _atkBase;
    [SerializeField] protected float _atkRange;
    [SerializeField] protected float _atkTime;
    [SerializeField] protected float _atkTimer;

    [SerializeField] protected int _upgrade;
    [SerializeField] protected int _upgradeMax;
    [SerializeField] protected int _upgradeMin;
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

    protected void FixedUpdate()
    {
        if (_target == null)
        {
            _target = Search();
        }

        LaunchMissle();
    }

    Transform Search()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, _atkRange, Vector2.zero, 0.0f, _targetLayer);

        if (hit.Length > 0)
        {
            Debug.Log($"Enemy Found! Target : {hit[0].collider.name}");

            return hit[0].transform;
        }

        return null;
    }

    void LaunchMissle()
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

    public void SetUp(int randomNum)
    {
        id = TowerData.instance.towerDataList[randomNum].id;
        _towerType = TowerData.instance.towerDataList[randomNum].towerType;
        _attackType = TowerData.instance.towerDataList[randomNum].attackType;
        level = TowerData.instance.towerDataList[randomNum].level;
        _atkBase = TowerData.instance.towerDataList[randomNum].atkBase;
        _atkRange = TowerData.instance.towerDataList[randomNum].atkRange;
        _atkTime = TowerData.instance.towerDataList[randomNum].atkTime;
        _spriteRenderer.sprite = TowerData.instance.towerDataList[randomNum].sprite;
        _spriteRenderer.color = TowerData.instance.towerDataList[randomNum].color;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _atkRange);
    }
}