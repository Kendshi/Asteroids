using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private int _InitialNumberOfAsteroid = 2;
    [SerializeField] private float _minAsteroidSpeed = 1f;
    [SerializeField] private float _maxAsterodSpeed = 2f;
    [SerializeField] private float _delayBeforeSpawn = 2f;

    [SerializeField] private BigAsteroidsPool _bigAsteroidsPool;
    [SerializeField] private OtherAsteroidsPool _middleAsteroidsPool;
    [SerializeField] private OtherAsteroidsPool _smallAsteroidsPool;
    [SerializeField] private Boudry[] SpawnZones;
    [SerializeField] private Transform target;

    private int _asteroidsCountToSpawn;
    private int _numberOfAsteroidsOnScene;
    private Vector3 _spawnPosition;

    private void Awake()
    {
        _numberOfAsteroidsOnScene = 0;
        _asteroidsCountToSpawn = _InitialNumberOfAsteroid;
        BigAsteroid.CancelSubscribe += CancelSubscribe;
        OtherAsteroid.CancelSubscribe += CancelSubscribe;
    }

    public void RestartGame()
    {
        _asteroidsCountToSpawn = 2;
        _bigAsteroidsPool.DeactivateAllElements();
        _middleAsteroidsPool.DeactivateAllElements();
        _smallAsteroidsPool.DeactivateAllElements();
        SpawnFirstAsteroidsInRound();
    }

    public void SpawnFirstAsteroidsInRound()
    {
        StartCoroutine(DelayBeforeSpawn());
    }

    private void SpawnBigAsteroids()
    {
        for (int i = 0; i < _asteroidsCountToSpawn; i++)
        {
            Boudry zone = SpawnZones[Random.Range(0, SpawnZones.Length)];
            if (zone.CurrentPosition == Boudry.Position.Horizontal)
            {
                float x = Random.Range(-zone.transform.localScale.x / 2, zone.transform.localScale.x / 2);
                _spawnPosition = new Vector3(x, zone.transform.position.y, 0);
            }
            else
            {
                float y = Random.Range(-zone.transform.localScale.y / 2, zone.transform.localScale.y / 2);
                _spawnPosition = new Vector3(zone.transform.position.x, y, 0);
            }

            BigAsteroid asteroid = _bigAsteroidsPool.ActivatePoolElement();
            ++_numberOfAsteroidsOnScene;
            asteroid.transform.position = _spawnPosition;
            asteroid.AsteroidDestroy += SpawnsAsteroids;
            asteroid.CollisionWithSpaceShip += ChangeNumberOfAsteroids;
        }

        ++_asteroidsCountToSpawn;
    }

    public void CancelSubscribe(Asteroid asteroid)
    {
        asteroid.AsteroidDestroy -= SpawnsAsteroids;
        asteroid.CollisionWithSpaceShip -= ChangeNumberOfAsteroids;
    }

    private void CheckEndOfRound()
    {
        --_numberOfAsteroidsOnScene;

        if (_numberOfAsteroidsOnScene == 0)
            SpawnFirstAsteroidsInRound();
    }

    private void SpawnsAsteroids(Asteroid asteroid, Asteroid.AsteroidSize size, Vector3 position, float rotation)
    {
        asteroid.AsteroidDestroy -= SpawnsAsteroids;
        asteroid.CollisionWithSpaceShip -= ChangeNumberOfAsteroids;
        switch (size)
        {
            case Asteroid.AsteroidSize.BigAsteroid:
                SpawnAsteroids(false, position, rotation);
                break;
            case Asteroid.AsteroidSize.MiddleAsteroid:
                SpawnAsteroids(true, position, rotation);
                break;
            case Asteroid.AsteroidSize.SmallAsteroid:
                CheckEndOfRound();
                break;
        }
    }

    private void SpawnAsteroids(bool spawnSmallAsteroid, Vector3 position, float rotation)
    {
        --_numberOfAsteroidsOnScene;
        float movementSpeed = Random.Range(_minAsteroidSpeed, _maxAsterodSpeed);

        for (int i = 0; i < 2; i++)
        {
            OtherAsteroid asteroid;
            if (spawnSmallAsteroid)
                asteroid = _smallAsteroidsPool.ActivatePoolElement();
            else
                asteroid = _middleAsteroidsPool.ActivatePoolElement();

            asteroid.AsteroidDestroy += SpawnsAsteroids;
            Vector3 offset = new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), 0);
            asteroid.transform.position = position + offset;
            asteroid.SetRotation(rotation, movementSpeed);
            asteroid.CollisionWithSpaceShip += ChangeNumberOfAsteroids;
            ++_numberOfAsteroidsOnScene;
        }
    }

    private void ChangeNumberOfAsteroids(Asteroid asteroid)
    {
        CancelSubscribe(asteroid);
        CheckEndOfRound();
    }

    private IEnumerator DelayBeforeSpawn()
    {
        yield return new WaitForSeconds(_delayBeforeSpawn);
        SpawnBigAsteroids();
    }

    private void OnDisable()
    {
        StopCoroutine(DelayBeforeSpawn());
    }

    private void OnDestroy()
    {
        BigAsteroid.CancelSubscribe -= CancelSubscribe;
        OtherAsteroid.CancelSubscribe -= CancelSubscribe;
    }
}
