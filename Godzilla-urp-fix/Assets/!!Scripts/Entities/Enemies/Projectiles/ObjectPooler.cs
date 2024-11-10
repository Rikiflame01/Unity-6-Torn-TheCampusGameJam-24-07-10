using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler<T> where T : MonoBehaviour
{
    private Queue<T> _objectPool = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _parentTransform;

    public ObjectPooler(T prefab, int initialSize, Transform parentTransform = null)
    {
        _prefab = prefab;
        _parentTransform = parentTransform;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(_prefab, _parentTransform);
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        while (_objectPool.Count > 0)
        {
            T obj = _objectPool.Dequeue();

            if (obj != null)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogWarning("Null object found in pool, removing.");
            }
        }

        T newObj = Object.Instantiate(_prefab, _parentTransform);
        newObj.gameObject.SetActive(true);
        return newObj;
    }

    public void ReturnObject(T obj)
    {
        if (obj != null)
        {
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Attempted to return a null object to the pool.");
        }
    }

    public void RemoveNullObjects()
    {
        int initialCount = _objectPool.Count;
        _objectPool = new Queue<T>(_objectPool.Where(o => o != null));
        int removedCount = initialCount - _objectPool.Count;

        if (removedCount > 0)
        {
            Debug.LogWarning($"Removed {removedCount} null objects from the pool.");
        }
    }
}
