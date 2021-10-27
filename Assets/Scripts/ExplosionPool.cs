using UnityEngine;

public class ExplosionPool : BasePool<ExplosionEffect>
{
    private PoolMono<ExplosionEffect> _pool;

    private void Start()
    {
        _pool = new PoolMono<ExplosionEffect>(_poolObject, _poolObjectCount, this.transform);
    }

    public void ActivatePoolElement(Vector3 position, Vector3 scale)
    {
        var effect = _pool.GetFreeElement();
        effect.transform.position = position;
        effect.transform.localScale = scale;
    }

    public void DeactivateAllElements()
    {
        _pool.DeactivateAllElements();
    }
}
