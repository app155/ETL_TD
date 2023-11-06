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
        _target = _owner._target;
        _targetPos = _target.position;
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

    void RefreshTargetPosition()
    {
        _targetPos = _target.position;
    }

    void Follow(Vector2 targetPos)
    {
        _rigid.position += (targetPos - _rigid.position).normalized * _moveSpeed * Time.fixedDeltaTime;
    }
}
