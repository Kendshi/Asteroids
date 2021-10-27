using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerStartPoint;
    [SerializeField] private BulletPool _playerBulletPool;
    [SerializeField] private InputFromPlayer _input;
    [SerializeField] private Menu _menu;
    [SerializeField] private AsteroidSpawner _asteroidSpawner;
    [SerializeField] private ExplosionPool _explosionPool;
    [SerializeField] private Score _score;
    [SerializeField] private UFOSpawner _UFOSpawner;

    private GameObject _playerShip;

    private void Start()
    {
        _input.EnableMenu += EnableMenu;
    }

    public void StartGame()
    {
        _playerShip = Instantiate(_playerPrefab, _playerStartPoint.position, Quaternion.identity);
        _playerShip.GetComponent<PlayerMove>().Init(_input);
        _playerShip.GetComponent<PlayerShooting>().Init(_input, _playerBulletPool);
        Player player = _playerShip.GetComponent<Player>();
        player.Init(_explosionPool);
        player.GameOver += RestartGame;
        _asteroidSpawner.SpawnFirstAsteroidsInRound();
        _UFOSpawner.Init(_playerShip.transform);
    }

    public void RestartGame()
    {
        _playerShip.transform.SetPositionAndRotation(_playerStartPoint.position, Quaternion.identity);
        _playerShip.GetComponent<PlayerMove>().StopMovement();
        _playerBulletPool.DeactivateAllElements();
        _explosionPool.DeactivateAllElements();
        _asteroidSpawner.RestartGame();
        _score.RestartScore();
        _UFOSpawner.RestartGame();
        _playerShip.GetComponent<Player>().RestartGame();
    }

    private void EnableMenu()
    {
        _menu.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _input.EnableMenu -= EnableMenu;
    }
}
