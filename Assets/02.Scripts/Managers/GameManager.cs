using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    None,
    BeforeStart,
    BuildPhase,
    DefensePhase,
    GamePaused,
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
                gamePhase = GamePhase.GameOver;
            }
        }
    }

    public GamePhase gamePhase
    {
        get { return _gamePhase; }
        set
        {
            if (_gamePhase == value)
                return;

            if (value == GamePhase.GamePaused)
            {
                _prevGamePhase = _gamePhase;
            }

            _gamePhase = value;

            switch (value)
            {
                case GamePhase.GamePaused:
                    onGamePaused?.Invoke();
                    break;
                case GamePhase.GameOver:
                    onGameOver?.Invoke();
                    break;
                case GamePhase.GameClear:
                    onGameClear?.Invoke();
                    break;
            }
        }
    }

    private GamePhase _prevGamePhase;
    private GamePhase _gamePhase;
    private static GameManager _instance;
    [SerializeField] private int _initialGold;
    private int _gold;
    [SerializeField] private int _initialLife;
    private int _life;


    public int round;
    [SerializeField] private int _roundMax;


    public event Action onGoldChanged;
    public event Action onLifeDepleted;
    public event Action<string> onTextNotifyRequired;
    public event Action onDefencePhaseEnded;
    public event Action onGamePaused;
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
                gamePhase = GamePhase.GameClear;
            }    
        };

        onGamePaused += () =>
        {
            Time.timeScale = 0.0f;
        };

        onGameOver += () =>
        {
            Time.timeScale = 0.0f;
        };

        onGameClear += () =>
        {
            Time.timeScale = 0.0f;
        };
    }

    void Start()
    {
        gold = _initialGold;
        life = _initialLife;
        gamePhase = GamePhase.BeforeStart;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            onTextNotifyRequired?.Invoke("A downed");

        if (Input.GetKeyDown(KeyCode.S))
            onTextNotifyRequired?.Invoke("S downed");

        if (Input.GetKeyDown(KeyCode.D))
            onTextNotifyRequired?.Invoke("D downed");
    }

    public void EndDefensePhase()
    {
        onDefencePhaseEnded?.Invoke();
    }

    public void EndGamePause()
    {
        _gamePhase = _prevGamePhase;
        Time.timeScale = 1.0f;
    }

}
