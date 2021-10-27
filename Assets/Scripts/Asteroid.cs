using UnityEngine;
using Random = UnityEngine.Random;
using System;

public abstract class Asteroid : MonoBehaviour
{
    public enum AsteroidSize
    {
        BigAsteroid,
        MiddleAsteroid,
        SmallAsteroid
    }

    public static Action<Score.RewardType> SetReward;
    public virtual event Action<Asteroid> CollisionWithSpaceShip;
    public virtual event Action<Asteroid, AsteroidSize, Vector3, float> AsteroidDestroy;

    [SerializeField] protected AsteroidSize _asteroidSize;
    [SerializeField] protected Score.RewardType _rewardType;
    [SerializeField] protected float _minSize;
    [SerializeField] protected float _maxSize;

    [HideInInspector] public ExplosionPool _explosionPool;
    [HideInInspector] public int TeleportNumber => _teleportNumber;

    protected Rigidbody2D _rigidbody;
    protected Quaternion _direction;
    protected Transform _spriteTransform;
    protected float _rotationSpeed;
    protected int _teleportNumber;


    protected void Init()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody2D>();

        if (_spriteTransform == null)
            _spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;

        _rotationSpeed = Random.Range(15f, 25f);

        RestarTeleportNumber();
    }

    public void IncreaseTeleportNumber()
    {
        ++_teleportNumber;
    }

    public void RestarTeleportNumber()
    {
        _teleportNumber = 0;
    }

    private void Update()
    {
        _spriteTransform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime);
    }

    protected abstract void SetMotion();

    protected void Deactivate()
    {
        gameObject.SetActive(false);
    }

    protected void CollisionEvent()
    {
        _explosionPool.ActivatePoolElement(transform.position, Vector3.one);
        SoundController.Instance.PlaySound(SoundController.SoundType.Explosion);
        Deactivate();
    }
}
