using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T : MonoBehaviour
{
    public T Prefab { get; }
    public Transform Container { get; }

    private List<T> _pool;

    public PoolMono(T prefab, int count, Transform container)
    {
        Prefab = prefab;
        Container = container;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(Prefab, Container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in _pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        return CreateObject(true);

        throw new System.Exception($"Нет свободного элемента в пуле типа {typeof(T)}");
    }

    public void DeactivateAllElements()
    {
        foreach (var mono in _pool)
        {
            mono.gameObject.SetActive(false);
        }
    }

    public List<T> GetAllElements()
    {
        return _pool;
    }
}
