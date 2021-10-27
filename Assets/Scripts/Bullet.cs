using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _lifeTime;
    private Transform _bullet;

    private void OnEnable()
    {
        if (_bullet == null)
        {
            _bullet = transform;
        }

        StartCoroutine(LifeRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(LifeRoutine());
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _bulletSpeed);
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage();
            SoundController.Instance.PlaySound(SoundController.SoundType.Explosion);
            gameObject.SetActive(false);
        }
    }
}
