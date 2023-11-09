using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    None,
    One,
    Two,
    Three,
}

public enum AttackType
{
    None,
    Normal,
    Splash,
}

public class TowerController : MonoBehaviour
{
    [SerializeField] private TowerType _towerType;
    public AttackType _attackType;

    public int id;
    [SerializeField] protected int level;
    public MapManager.TileInfo tileBelong;

    public int _atk;
    [SerializeField] protected int _atkBase;
    [SerializeField] protected float _atkRange;
    [SerializeField] protected float _atkTime;
    [SerializeField] protected float _atkTimer;

    [SerializeField] protected int _upgrade;
    [SerializeField] protected int _upgradeMax;
    [SerializeField] protected float _upgradeGain;

    public Transform _target;
    [SerializeField] protected MissleController _missle;
    [SerializeField] protected LayerMask _targetLayer;

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
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

    public void SetUp(int id)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _atkRange);
    }
}
