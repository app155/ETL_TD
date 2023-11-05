using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool : MonoBehaviour
{
    class PooledItem : MonoBehaviour
    {
        public IObjectPool<GameObject> pool;
        public Action onReturnToPool;

        private void OnDisable()
        {
            ReturnToPool();
        }

        public void ReturnToPool()
        {
            pool.Release(gameObject);
            onReturnToPool?.Invoke();
        }
    }

    [SerializeField] protected int id;
    [SerializeField] private bool _collectionCheck;

    public IObjectPool<GameObject> pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<GameObject>(CreatePooledItem,
                                                   OnGetFromPool,
                                                   OnReturnToPool,
                                                   OnDestroyPooledItem,
                                                   _collectionCheck,
                                                   _count,
                                                   _countMax);
            }

            return _pool;
        }
    }

    private IObjectPool<GameObject> _pool;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _count;
    [SerializeField] private int _countMax;

    protected virtual void Start()
    {
        PoolManager.instance.Register(id, pool);
    }

    protected virtual GameObject CreatePooledItem()
    {
        GameObject item = Instantiate(_prefab);
        item.AddComponent<PooledItem>().pool = pool;

        return item;
    }

    protected virtual void OnGetFromPool(GameObject item)
    {
        item.SetActive(true);
    }

    protected virtual void OnReturnToPool(GameObject item)
    {
        gameObject.SetActive(false);
    }
    protected virtual void OnDestroyPooledItem(GameObject item)
    {
        Destroy(gameObject);
    }
}
