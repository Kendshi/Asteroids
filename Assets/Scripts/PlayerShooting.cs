using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float _rechargeTime;
    [SerializeField] private Transform _gunPoint;

    public BulletPool _bulletPool;
    private InputFromPlayer _input;
    private bool _canNotShoot;

    public void Init(InputFromPlayer input, BulletPool pool)
    {
        input.Shoot += Shoot;
        _bulletPool = pool;
    }

    private void Shoot()
    {
        if (!_canNotShoot)
        {
            _canNotShoot = true;
            SoundController.Instance.PlaySound(SoundController.SoundType.Shoot);
            _bulletPool.ActivatePoolElement(_gunPoint.position, transform.rotation, true);
            StartCoroutine(DelayBetweenShots());
        }
    }

    private IEnumerator DelayBetweenShots()
    {
        yield return new WaitForSeconds(_rechargeTime);
        _canNotShoot = false;
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Shoot -= Shoot;
        }
    }
}
