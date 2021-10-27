using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class UFO : MonoBehaviour
{
    private const float MOVEMENT_SPEED = 1.8f;

    public static Action<Score.RewardType> SetReward;
    public event Action UFODefeat;

    [SerializeField] private ExplosionPool _explosionPool;

    private int _directionNumber;
    private Transform _UFOTransform;
    private Vector3[] _directions = new Vector3[2]
    {
        Vector3.right,
        Vector3.left
    };

    private void Start()
    {
        _UFOTransform = transform;
    }

    private void OnEnable()
    {
        _directionNumber = Random.Range(0, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
        {
            bullet.gameObject.SetActive(false);
            SetReward?.Invoke(Score.RewardType.UFO);
            CollisionEvent();
        }
        if (collision.TryGetComponent(out Player player))
        {
            player.TakeDamage();
            CollisionEvent();
        }
    }

    private void CollisionEvent()
    {
        UFODefeat?.Invoke();
        _explosionPool.ActivatePoolElement(transform.position, Vector3.one);
        SoundController.Instance.PlaySound(SoundController.SoundType.Explosion);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _UFOTransform.Translate(_directions[_directionNumber] * Time.deltaTime * MOVEMENT_SPEED);
    }
}
