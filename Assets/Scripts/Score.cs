using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public enum RewardType
    {
        BigAsteroid,
        MiddleAsteroid,
        SmallAsteroid,
        UFO
    }

    [SerializeField] private Text _scoreText;
    [SerializeField] private int _rewardBigAsteroid;
    [SerializeField] private int _rewardMiddleAsteroid;
    [SerializeField] private int _rewardSmallAsteroid;
    [SerializeField] private int _rewardUFO;

    private int _score;

    private void Start()
    {
        Asteroid.SetReward += SetReward;
        UFO.SetReward += SetReward;
    }

    public void RestartScore()
    {
        _score = 0;
        UpdateScore();
    }

    private void SetReward(RewardType reward)
    {
        switch (reward)
        {
            case RewardType.BigAsteroid:
                _score += _rewardBigAsteroid;
                break;
            case RewardType.MiddleAsteroid:
                _score += _rewardMiddleAsteroid;
                break;
            case RewardType.SmallAsteroid:
                _score += _rewardSmallAsteroid;
                break;
            case RewardType.UFO:
                _score += _rewardSmallAsteroid;
                break;
        }

        UpdateScore();
    }

    private void UpdateScore()
    {
        string specifier = "00000";
        _scoreText.text = _score.ToString(specifier);
    }
}