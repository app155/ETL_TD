using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public static TowerData instance => _instance;

    private static TowerData _instance;

    public List<TowerDataSO> towerDataList;

    void Awake()
    {
        //if (_instance != null)
        //{
        //    Destroy(gameObject);
        //}
        //_instance = this;

        //DontDestroyOnLoad(gameObject);

        _instance = this;
    }
}
