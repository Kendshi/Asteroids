using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{
    public enum SoundType
    {
        Shoot,
        Thrust,
        Explosion
    }

    [SerializeField] private AudioSource[] _audioSources;

    public static SoundController Instance;

    private bool _stopPlaying;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _stopPlaying = false;
    }

    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.Shoot:
                _audioSources[0].Play();
                break;
            case SoundType.Explosion:
                _audioSources[2].Play();
                break;
        }
    }

    public void PlayLoopingSound()
    {
        if (!_stopPlaying)
        {
            StartCoroutine(PlayThrustSound());
        }
    }

    private IEnumerator PlayThrustSound()
    {
        _stopPlaying = true;
        _audioSources[1].Play();
        yield return new WaitForSeconds(_audioSources[1].clip.length);
        _stopPlaying = false;
    }

}
