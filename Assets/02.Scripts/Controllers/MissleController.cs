using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : MonoBehaviour
{
    public TowerController owner;
    protected AttackType _attackType;

    public int id;
    public int _atk;
    protected float _moveSpeed;
    public Transform _target;
    protected Vector2 _targetPos;

    protected Rigidbody2D _rigid;
    protected LayerMask _targetLayer;
    protected SpriteRenderer _spriteRenderer;
    protected Color _color;

    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    protected void Update()
    {
        if (_target != null && _target.gameObject.activeSelf)
        {
            RefreshTargetPosition();
        }
    }

    protected void FixedUpdate()
    {
        Follow(_targetPos);
    }

    public virtual void SetUp()
    {
        transform.position = owner.transform.position;
        _atk = owner.atk;
        _attackType = owner.attackType;
        _target = owner._target;
        _targetPos = _target.position;
        id = MissleData.instance.missleDataList[owner.id].id;
        _moveSpeed = MissleData.instance.missleDataList[id].moveSpeed;
        _spriteRenderer.sprite = MissleData.instance.missleDataList[id].sprite;
        _spriteRenderer.color = MissleData.instance.missleDataList[id].color;
        _targetLayer = MissleData.instance.missleDataList[id].targetLayer;
    }

    protected void RefreshTargetPosition()
    {
        _targetPos = _target.position;

        if (_target.gameObject.activeSelf == false)
            _target = null;
    }

    protected void Follow(Vector2 targetPos)
    {
        Vector2 expectedPos = (targetPos - _rigid.position).normalized * _moveSpeed * Time.fixedDeltaTime;
        float expectedDistance = expectedPos.magnitude;

        if (Vector2.Distance(_targetPos, _rigid.position) < expectedDistance)
            _rigid.position = targetPos;

        else
        {
            _rigid.position += expectedPos;
        }

        if (Vector2.Distance(_targetPos, _rigid.position) < 0.03f)
        {
            Fire();
        }
    }

    protected virtual void Fire()
    {
        gameObject.SetActive(false);
    }
}
