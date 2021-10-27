using UnityEngine;
using System.Collections.Generic;

public class BigAsteroidsPool : BasePool<BigAsteroid>
{
    [SerializeField] ExplosionPool _explosionPool;
    private PoolMono<BigAsteroid> _pool;

    private void Start()
    {
        _pool = new PoolMono<BigAsteroid>(_poolObject, _poolObjectCount, this.transform);
    }

    public BigAsteroid ActivatePoolElement()
    {
        var asteroid = _pool.GetFreeElement();

        if (asteroid._explosionPool == null)
            asteroid._explosionPool = _explosionPool;

        return asteroid;
    }

    public void DeactivateAllElements()
    {
        foreach (var asteroid in _pool.GetAllElements())
        {
            asteroid.RestartGame();
        }

        _pool.DeactivateAllElements();
    }

}
