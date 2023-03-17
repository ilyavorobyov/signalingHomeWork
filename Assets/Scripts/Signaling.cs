using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _volume = 0;
    private float _desiredVolume = 1;
    private float _volumeStep = 0.15f;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _audioSource.volume = _volume;
        _volume = Mathf.MoveTowards(_volume, _desiredVolume, _volumeStep * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            _desiredVolume = 1;
            _audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            _desiredVolume = 0;
            _audioSource.Play();
        }
    }
}