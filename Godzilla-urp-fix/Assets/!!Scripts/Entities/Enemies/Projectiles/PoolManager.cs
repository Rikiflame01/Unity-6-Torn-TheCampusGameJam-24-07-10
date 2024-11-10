using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private readonly Dictionary<string, object> _pools = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool<T>(string poolKey, T prefab, int initialSize, Transform parentTransform = null) where T : MonoBehaviour
    {
        if (!_pools.ContainsKey(poolKey))
        {
            ObjectPooler<T> pool = new ObjectPooler<T>(prefab, initialSize, parentTransform);
            _pools.Add(poolKey, pool);
        }
    }

    public bool HasPool(string poolName)
    {
        return _pools.ContainsKey(poolName);
    }

    public T GetObject<T>(string poolKey) where T : MonoBehaviour
    {
        if (_pools.ContainsKey(poolKey))
        {
            ObjectPooler<T> pool = (ObjectPooler<T>)_pools[poolKey];
            T obj = pool.GetObject();

            while (obj == null)
            {
                Debug.LogWarning($"Null object detected in pool '{poolKey}'. Removing null objects.");
                pool.RemoveNullObjects();

                obj = pool.GetObject();

                if (obj == null)
                {
                    Debug.LogError($"No valid objects available in pool '{poolKey}' after removing nulls.");
                    break;
                }
            }

            return obj;
        }
        else
        {
            Debug.LogError($"Pool with key '{poolKey}' does not exist.");
            return null;
        }
    }

    public void ReturnObject<T>(string poolKey, T obj) where T : MonoBehaviour
    {
        if (_pools.ContainsKey(poolKey))
        {
            ((ObjectPooler<T>)_pools[poolKey]).ReturnObject(obj);
        }
        else
        {
            Debug.LogError($"Pool with key '{poolKey}' does not exist.");
        }
    }


}
