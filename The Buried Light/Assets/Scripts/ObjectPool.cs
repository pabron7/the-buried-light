using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T newObject = Object.Instantiate(_prefab, _parent);
        newObject.gameObject.SetActive(false);
        _pool.Enqueue(newObject);
        return newObject;
    }

    public T GetObject()
    {
        if (_pool.Count == 0)
        {
            CreateNewObject();
        }

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    public void ClearPool()
    {
        while (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            Object.Destroy(obj.gameObject);
        }
    }
}
