using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<int[]> path;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _step;

    private int[] _nextPosIndex;
    [SerializeField] private Vector3 nextPos;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        path = new List<int[]>(MapManager.instance.path);
        transform.position = new Vector2(-8.5f, 4.5f);
        _spriteRenderer.sprite = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].sprite;
        _moveSpeed = RoundEnemyData.instance.enemyDataList[GameManager.instance.round].moveSpeed;
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

    void MoveByPath()
    {
        _nextPosIndex = path[_step];
        Vector3 nextPos = MapManager.instance._map[_nextPosIndex[0], _nextPosIndex[1]].tilePos;

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
