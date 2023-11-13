using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    None,
    BuildPhase,
    DefensePhase,
    GameOver,
    GameClear,
}

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

            if (_life <= 0)
            {
                onGameOver?.Invoke();
            }
        }
    }

    public GamePhase gamePhase;

    private static GameManager _instance;
    [SerializeField] private int _initialGold;
    private int _gold;
    [SerializeField] private int _initialLife;
    private int _life;


    public int round;
    [SerializeField] private int _roundMax;


    public event Action onGoldChanged;
    public event Action onLifeDepleted;
    public event Action onDefencePhaseEnded;
    public event Action onGameOver;
    public event Action onGameClear;

    private void Awake()
    {
        _instance = this;
        onDefencePhaseEnded += () =>
        {
            gamePhase = GamePhase.BuildPhase;
            round++;

            if (round > _roundMax)
            {
                onGameClear?.Invoke();
            }    
        };

        onGameOver += () =>
        {
            gamePhase = GamePhase.GameOver;
            Time.timeScale = 0.0f;
        };

        onGameClear += () =>
        {
            gamePhase = GamePhase.GameClear;
            Time.timeScale = 0.0f;
        };
    }

    void Start()
    {
        gold = _initialGold;
        life = _initialLife;
        gamePhase = GamePhase.BuildPhase;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            round++;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            PoolManager.instance.Get((int)PoolTag.Enemy);

        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log(gamePhase);
    }

    public void EndDefensePhase()
    {
        onDefencePhaseEnded?.Invoke();
    }

    public void GameClear()
    {

    }
}
