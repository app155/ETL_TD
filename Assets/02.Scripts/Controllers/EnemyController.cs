using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp
    {
        get { return _hp; }
        set
        {
            if (_hp <= 0)
            {
                _hp = 0;
            }

            else
            {
                _hp -= value;
            }
        }
    }

    private List<int[]> path;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _step;
    private int _hp;

    private int[] _nextPosIndex;
    [SerializeField] private Vector3 nextPos;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        SetUp();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveByPath();
    }

    public void SetUp()
    {
        path = new List<int[]>(MapManager.instance.path);
        transform.position = new Vector2(-8.5f, 4.5f);
        _spriteRenderer.sprite = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].sprite;
        _moveSpeed = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].moveSpeed;
    }

    void MoveByPath()
    {
        _nextPosIndex = path[_step];
        Vector3 nextPos = MapManager.instance.map[_nextPosIndex[0], _nextPosIndex[1]].tilePos;

        if (Vector2.Distance(transform.position, nextPos) >= 0.03f)
        {
            transform.position += (nextPos - transform.position).normalized * _moveSpeed * Time.deltaTime;
        }

        else
        {
            _step++;
        }
    }
}
