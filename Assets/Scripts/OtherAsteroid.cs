using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class OtherAsteroid : Asteroid
{
    public static event Action<Asteroid> CancelSubscribe;
    public override event Action<Asteroid, AsteroidSize, Vector3, float> AsteroidDestroy;
    public override event Action<Asteroid> CollisionWithSpaceShip;

    [SerializeField] private float _minNewRotation = -45f;
    [SerializeField] private float _maxNewRotation = 45f;

    private float _newRotation;
    private float _movementSpeed;
    private Vector3 scale;

    private void OnEnable()
    {
        Init();
        SetSize();
    }

    public void SetRotation(float oldRotation, float movementSpeed)
    {
        float randomizer = Random.Range(_minNewRotation, _maxNewRotation);
        _newRotation = oldRotation + randomizer;
        _movementSpeed = movementSpeed;
        SetMotion();
    }

    public void RestartGame()
    {
        CancelSubscribe?.Invoke(this);
    }

    protected override void SetMotion()
    {
        _rigidbody.rotation = _newRotation;
        _rigidbody.AddRelativeForce(Vector2.up * _movementSpeed, ForceMode2D.Impulse);
    }

    private void SetSize()
    {
        if (_asteroidSize == AsteroidSize.SmallAsteroid)
        {
            float randomSize = Random.Range(_minSize, _maxSize);
            scale = new Vector3(randomSize, randomSize, 1f);
        }
        else if (_asteroidSize == AsteroidSize.MiddleAsteroid)
        {
            float randomSize = Random.Range(_minSize, _maxSize);
            scale = new Vector3(randomSize, randomSize, 1f);
        }

        transform.localScale = scale;
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
            AsteroidDestroy?.Invoke(this, _asteroidSize, transform.position, _rigidbody.rotation);
            SetReward?.Invoke(_rewardType);

            CollisionEvent();
        }

        if (collision.TryGetComponent(out UFO UFOShip))
        {
            UFOShip.gameObject.SetActive(false);
            CollisionWithSpaceShip?.Invoke(this);

            CollisionEvent();
        }
    }
}
