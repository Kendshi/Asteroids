using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Text _healthCount;

    private void Start()
    {
        Player.HealthChange += ChangeHealtCount;
    }

    private void ChangeHealtCount(int health)
    {
        _healthCount.text = health.ToString();
    }

    private void OnDestroy()
    {
        Player.HealthChange -= ChangeHealtCount;
    }
}
