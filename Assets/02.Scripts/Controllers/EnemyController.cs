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
            _hp = value;

            if (_hp <= 0)
            {
                _hp = 0;
                Die();
            }
        }
    }

    private List<int[]> path;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _step;
    [SerializeField] private int _hp;
    [SerializeField] private int _hpMax;

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
        _hp = _hpMax;
        _step = 0;
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

            if (_step >= path.Count)
            {
                SpawnManager.instance.enemyRemainCount--;
                GameManager.instance.life--;
                gameObject.SetActive(false);
            }
        }
    }

    public void DepleteHp(int value)
    {
        hp -= value;
    }

    void Die()
    {
        SpawnManager.instance.enemyRemainCount--;
        GameManager.instance.gold += 1;
        gameObject.SetActive(false);
    }
}
