using UnityEngine;

public class BulletPool : BasePool<Bullet>
{
    private PoolMono<Bullet> _pool;

    private void Start()
    {
        _pool = new PoolMono<Bullet>(_poolObject, _poolObjectCount, this.transform);
    }

    public void ActivatePoolElement(Vector3 position, Quaternion rotate, bool isPlayer)
    {
        var bullet = _pool.GetFreeElement();
        bullet.transform.position = position;
        bullet.transform.rotation = rotate;

        if (isPlayer)
        {
            bullet.GetComponent<SpriteRenderer>().color = Color.green;
            bullet.gameObject.layer = 7;
        }
        else
        {
            bullet.GetComponent<SpriteRenderer>().color = Color.red;
            bullet.gameObject.layer = 12;
        }
    }

    public void DeactivateAllElements()
    {
        _pool.DeactivateAllElements();
    }
}
