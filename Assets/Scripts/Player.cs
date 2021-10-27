using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(EdgeCollider2D))]
public class Player : MonoBehaviour
{
    private const int START_HEALTH_NUMBER = 4;

    public static event Action<int> HealthChange;
    public event Action GameOver;

    private int _health;
    private ExplosionPool _explosionPool;
    private Animation _deathAnimation;
    private EdgeCollider2D _collider;
    private PlayerMove _playerMove;
    private float _immortalTime;
    private Transform _playerTransform;
    private Vector3 _explosionScale = new Vector3(0.8f, 0.8f, 1f);

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _collider = GetComponent<EdgeCollider2D>();
        _deathAnimation = GetComponent<Animation>();
        _immortalTime = _deathAnimation.clip.length;
        _playerTransform = transform;
    }

    public void Init(ExplosionPool pool)
    {
        _explosionPool = pool;
        _health = START_HEALTH_NUMBER;
        HealthChange?.Invoke(_health);
    }

    public void TakeDamage()
    {
        --_health;
        _playerMove.StopMovement();
        _explosionPool.ActivatePoolElement(_playerTransform.position, _explosionScale);
        _playerTransform.position = Vector3.zero;
        HealthChange?.Invoke(_health);
        CheckGameOver();
        StartCoroutine(EnableImmortal());
    }

    public void RestartGame()
    {
        _health = START_HEALTH_NUMBER;
        HealthChange?.Invoke(_health);
    }

    private void CheckGameOver()
    {
        if (_health == 0)
        {
            RestartGame();
            GameOver?.Invoke();
        }
    }

    private IEnumerator EnableImmortal()
    {
        _collider.enabled = false;
        _deathAnimation.Play();
        yield return new WaitForSeconds(_immortalTime);
        _collider.enabled = true;
    }
}
