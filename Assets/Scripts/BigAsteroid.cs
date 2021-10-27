using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class BigAsteroid : Asteroid
{
    public static event Action<Asteroid> CancelSubscribe;
    public override event Action<Asteroid, AsteroidSize, Vector3, float> AsteroidDestroy;
    public override event Action<Asteroid> CollisionWithSpaceShip;

    [Header("Параметры большого астероида")]
    [SerializeField] private float _rotationRange;
    [SerializeField] private float _minMovementSpeed;
    [SerializeField] private float _maxMomementSpeed;

    private void OnEnable()
    {
        Init();
        SetMotion();
    }

    protected override void SetMotion()
    {
        _rigidbody.rotation = Random.Range(0, _rotationRange);
        float movementSpeed = Random.Range(_minMovementSpeed, _maxMomementSpeed);
        float randomSize = Random.Range(_minSize, _maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        _rigidbody.AddRelativeForce(Vector2.up * movementSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage();
            CollisionWithSpaceShip?.Invoke(this);

            CollisionEvent();
        }

        if (collision.TryGetComponent(out Bullet bullet))
        {
            bullet.gameObject.SetActive(false);
            SetReward?.Invoke(_rewardType);
            AsteroidDestroy?.Invoke(this, _asteroidSize, transform.position, _rigidbody.rotation);

            CollisionEvent();
        }

        if (collision.TryGetComponent(out UFO UFOShip))
        {
            UFOShip.gameObject.SetActive(false);
            CollisionWithSpaceShip?.Invoke(this);

            CollisionEvent();
        }
    }

    public void RestartGame()
    {
        CancelSubscribe?.Invoke(this);
    }
}
