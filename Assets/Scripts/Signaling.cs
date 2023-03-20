using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    [SerializeField] private EntrySensor _sensor;

    private AudioSource _audioSource;
    private float _volume = 0;
    private float _desiredVolume;
    private float _minVolume = 0;
    private float _maxVolume = 1;
    private float _volumeStep = 0.4f;
    private Coroutine _changeVolume;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _minVolume;
    }

    private IEnumerator ChangeVolume()
    {
        var waitFixedUpdate = new WaitForFixedUpdate();

        while (_audioSource.volume != _desiredVolume)
        {
            _volume = Mathf.MoveTowards(_volume, _desiredVolume, _volumeStep * Time.deltaTime);
            _audioSource.volume = _volume;

            if (_audioSource.volume == _maxVolume)
            {
                StopCoroutine(_changeVolume);
            }

            if (_audioSource.volume == _minVolume)
            {
                StopCoroutine(_changeVolume);
                _audioSource.Stop();
            }

            yield return waitFixedUpdate;
        }
    }

    private void Come()
    {
        _audioSource.Play();
        _desiredVolume = _maxVolume;

        if (_changeVolume != null)
        {
            StopCoroutine(_changeVolume);
        }

        _changeVolume = StartCoroutine(ChangeVolume());
    }

    private void Gone()
    {
        _desiredVolume = _minVolume;

        if (_changeVolume != null)
        {
            StopCoroutine(_changeVolume);
        }

        _changeVolume = StartCoroutine(ChangeVolume());
    }

    private void OnEnable()
    {
            _sensor.ThiefCame += Come;
            _sensor.ThiefGone += Gone;
    }

    private void OnDisable()
    {
            _sensor.ThiefCame -= Come;
            _sensor.ThiefGone -= Gone;
    }
}