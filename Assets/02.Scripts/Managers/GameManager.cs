using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public int gold
    {
        get { return _gold; }

        set
        {
            _gold = value;
            onGoldChanged?.Invoke();
        }
    }

    public int life
    {
        get { return _life; }

        set
        {
            _life = value;
            onLifeDepleted?.Invoke();
        }
    }

    private static GameManager _instance;
    [SerializeField] private int _initialGold;
    private int _gold;
    [SerializeField] private int _initialLife;
    private int _life;

    public int round;

    public Action onGoldChanged;
    public Action onLifeDepleted;

    void Start()
    {
        _gold = _initialGold;
        _life = _initialLife;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            round++;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            PoolManager.instance.Get((int)PoolTag.Enemy);
    }
}
