using UnityEngine;
using System.Collections;

public class UFOShooting : MonoBehaviour
{
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private Transform _gun;

    public Transform Target;

    private void OnEnable()
    {
        StartCoroutine(ShootDelay());
    }

    private void OnDisable()
    {
        StopCoroutine(ShootDelay());
    }

    private void Shoot()
    {
        CalculateGunDirection();
        SoundController.Instance.PlaySound(SoundController.SoundType.Shoot);
        _bulletPool.ActivatePoolElement(_gun.position, _gun.rotation, false);
    }

    private void CalculateGunDirection()
    {
        Vector3 aimDirection = (Target.position - _gun.position).normalized;
        float angle = Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg;
        float randomizer = Random.Range(-20f, 20f);
        Quaternion finalRotation = Quaternion.Euler(0, 0, -angle + randomizer);
        _gun.rotation = finalRotation;
    }

    private IEnumerator ShootDelay()
    {
        float delay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(delay);
        Shoot();
        StartCoroutine(ShootDelay());
    }
}
