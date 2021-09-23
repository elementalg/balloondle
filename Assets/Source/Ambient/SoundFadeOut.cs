using System;
using UnityEngine;

namespace Balloondle.Ambient
{
    /// <summary>
    /// Destroys the GameObject when the sound has been completely faded out.
    /// </summary>
    public class SoundFadeOut : MonoBehaviour
    {
        public float FadeOutDuration { get; set; } = 0.5f;

        private AudioSource _audioSource;
        private float _startingVolume;
        private float _elapsedTime;
        
        private void OnEnable()
        {
            if (GetComponent<AudioSource>() == null)
            {
                throw new InvalidOperationException("SoundFadeOut requires an AudioSource component.");
            }

            _audioSource = GetComponent<AudioSource>();
            _startingVolume = _audioSource.volume;
            _elapsedTime = 0f;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            float progress = _elapsedTime / FadeOutDuration;

            _audioSource.volume = Mathf.Min(0f, _startingVolume - _startingVolume * progress);
            
            if (progress >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}