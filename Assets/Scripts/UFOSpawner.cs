using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    private const float TOTAL_SCREEN_HIGH = 10;

    [SerializeField] private float _minTimeToSpawn;
    [SerializeField] private float _maxTimeToSpawn;
    [SerializeField] private UFO _UFO;
    [SerializeField] private Transform _spawnPosition;

    private float _timer;
    private bool _startDelay;

    private void Start()
    {
        _UFO.UFODefeat += StartLaunch;
    }

    public void Init(Transform player)
    {
        _UFO.GetComponent<UFOShooting>().Target = player;
        StartLaunch();
    }

    private void StartLaunch()
    {
        _timer = CalculateRemainTime();
        _startDelay = true;
    }

    public void RestartGame()
    {
        _UFO.gameObject.SetActive(false);
        StartLaunch();
    }

    private void CalculatePosition()
    {
        float y = Random.Range(-TOTAL_SCREEN_HIGH / 2 * 0.6f, TOTAL_SCREEN_HIGH / 2 * 0.6f);
        _UFO.transform.position = new Vector3(_spawnPosition.position.x, y, 0);
        _UFO.gameObject.SetActive(true);
    }

    private float CalculateRemainTime()
    {
        return Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
    }

    private void Update()
    {
        if (_startDelay)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _startDelay = false;
                CalculatePosition();
            }
        }
    }
}
