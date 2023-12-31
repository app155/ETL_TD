using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : ISceneListener
{
    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PoolManager();
                SceneManagerWrapped.instance.Register(_instance);
            }
                

            return _instance;
        }
    }

    private static PoolManager _instance;

    private Dictionary<int, IObjectPool<GameObject>> _pools = new Dictionary<int, IObjectPool<GameObject>>();

    public void Register(int id, IObjectPool<GameObject> pool)
    {
        _pools.Add(id, pool);
    }

    public GameObject Get(int id)
    {
        GameObject item = _pools[id].Get();

        return item;
    }

    public void OnBeforeSceneLoaded()
    {
        _pools.Clear();
    }

    public void OnAfterSceneLoaded()
    {

    }
}
