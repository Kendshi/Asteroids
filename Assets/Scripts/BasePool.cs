using UnityEngine;

public abstract class BasePool<T> : MonoBehaviour
{
    [SerializeField] protected int _poolObjectCount = 3;
    [SerializeField] protected T _poolObject;
    protected Transform _startPosition;
    protected Transform _rotate;

}
