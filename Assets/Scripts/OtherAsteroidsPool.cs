using UnityEngine;

public class OtherAsteroidsPool : BasePool<OtherAsteroid>
{
    [SerializeField] ExplosionPool _explosionPool;

    private PoolMono<OtherAsteroid> _pool;

    private void Start()
    {
        _pool = new PoolMono<OtherAsteroid>(_poolObject, _poolObjectCount, this.transform);
    }

    public OtherAsteroid ActivatePoolElement()
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
