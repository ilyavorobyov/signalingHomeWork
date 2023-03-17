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
/*    private IEnumerator _volumeUp;
    private IEnumerator _volumeDown;
*/
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _minVolume;
/*        _volumeUp = ChangeVolume();
        _volumeDown = VolumeDown();
*/    }

    private IEnumerator ChangeVolume()
    {
        _audioSource.Play();
        var waitFixedUpdate = new WaitForFixedUpdate();

        while (_audioSource.volume != _desiredVolume)
        {
            _volume = Mathf.MoveTowards(_volume, _desiredVolume, _volumeStep * Time.deltaTime);
            _audioSource.volume = _volume;

            if (_audioSource.volume == _minVolume || _audioSource.volume == _maxVolume)
            {
                StopCoroutine(ChangeVolume());
                _audioSource.priority = 22;
            }

            yield return waitFixedUpdate;
        }
    }

    private void OnEnable()
    {
        _sensors = FindObjectsOfType<EntrySensor>();

        foreach (var sensor in _sensors)
        {
            sensor.ThiefCame += IsCome;
            sensor.ThiefGone += IsGone;
        }
    }

    private void OnDisable()
    {
        foreach (var sensor in _sensors)
        {
            sensor.ThiefCame -= IsCome;
            sensor.ThiefGone -= IsGone;
        }
    }

    private void IsCome()
    {
        _desiredVolume = _maxVolume;
        StartCoroutine(ChangeVolume());
    }

    private void IsGone()
    {
        _desiredVolume = _minVolume;
        StartCoroutine(ChangeVolume());
    }
}