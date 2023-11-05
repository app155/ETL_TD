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
    [SerializeField] protected TowerType _towerType;
    [SerializeField] protected AttackType _attackType;

    [SerializeField] protected int id;
    [SerializeField] protected int level;

    [SerializeField] protected float _atk;
    [SerializeField] protected float _atkBase;
    [SerializeField] protected float _atkRange;
    [SerializeField] protected float _atkSpeed;

    [SerializeField] protected int _upgrade;
    [SerializeField] protected int _upgradeMax;
    [SerializeField] protected float _upgradeGain;

    [SerializeField] protected Transform _target;
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

        else
        {
            if (Vector2.Distance(_target.position, transform.position) > _atkRange)
            {
                _target = null;
            }
        }
    }

    Transform Search()
    {
        Debug.Log("Searching");

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
        Instantiate(_missle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _atkRange);
    }
}
