using UnityEngine;
using System.Collections;

public class ExplosionEffect : MonoBehaviour
{
    private ParticleSystem _explosion;
    private float _duration;

    private void OnEnable()
    {
        if (_explosion == null)
        {
            _explosion = GetComponent<ParticleSystem>();
            _duration = _explosion.main.duration;
        }
        _explosion.Play(true);
        StartCoroutine(LifeRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(LifeRoutine());
    }

    protected IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(_duration);
        gameObject.SetActive(false);
    }
}
