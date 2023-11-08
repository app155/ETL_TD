using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : MonoBehaviour
{
    public TowerController _owner;
    public AttackType attackType;

    public float _atk;
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
        _atk = _owner._atk;
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

    public void SetUp()
    {
        transform.position = _owner.transform.position;
        _target = _owner._target;
        _targetPos = _target.position;
    }

    void RefreshTargetPosition()
    {
        _targetPos = _target.position;
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

    void Fire()
    {
        // deal damage
        // effect
        gameObject.SetActive(false);
    }
}
