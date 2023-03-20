using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _volume = 0;
    private float _desiredVolume;
    private float _minVolume = 0;
    private float _maxVolume = 1;
    private float _volumeStep = 0.4f;
    private EntrySensor[] _sensors;
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

            if(_audioSource.volume == _minVolume)
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
        _changeVolume = StartCoroutine(ChangeVolume());
    }

    private void Gone()
    {
        _desiredVolume = _minVolume;
        _changeVolume = StartCoroutine(ChangeVolume());
    }

    private void OnEnable()
    {
        _sensors = FindObjectsOfType<EntrySensor>();

        foreach (var sensor in _sensors)
        {
            sensor.ThiefCame += Come;
            sensor.ThiefGone += Gone;
        }
    }

    private void OnDisable()
    {
        foreach (var sensor in _sensors)
        {
            sensor.ThiefCame -= Come;
            sensor.ThiefGone -= Gone;
        }
    }
}