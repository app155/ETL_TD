using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : MonoBehaviour
{
    public TowerController _owner;
    public AttackType attackType;

    public int _atk;
    public float _moveSpeed;
    public Transform _target;
    [SerializeField] private Vector2 _targetPos;

    private Rigidbody2D _rigid;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SetUp();
    }

    private void Update()
    {
        if (_target != null && _target.gameObject.activeSelf)
        {
            RefreshTargetPosition();
        }
    }

    private void FixedUpdate()
    {
        Follow(_targetPos);
    }

    public virtual void SetUp()
    {
        transform.position = _owner.transform.position;
        _atk = _owner.atk;
        _target = _owner._target;
        _targetPos = _target.position;
    }

    void RefreshTargetPosition()
    {
        _targetPos = _target.position;

        if (_target.gameObject.activeSelf == false)
            _target = null;
    }

    void Follow(Vector2 targetPos)
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
