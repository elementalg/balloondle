using System;
using UnityEngine;

namespace Balloondle.Ambient
{
    /// <summary>
    /// Fades in an <see cref="AudioSource"/>. When fade is completed, this component proceeds to destroy itself.
    /// </summary>
    public class SoundFadeIn : MonoBehaviour
    {
        public float FadeInDuration { get; set; } = 0.5f;
        public float Volume { get; set; } = 1f;

        private AudioSource _audioSource;
        private float _elapsedTime;
        
        private void OnEnable()
        {
            if (GetComponent<AudioSource>() == null)
            {
                throw new InvalidOperationException("SoundFadeIn requires an AudioSource component.");
            }

            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = 0f;
            _elapsedTime = 0f;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            float progress = _elapsedTime / FadeInDuration;

            _audioSource.volume = Mathf.Min(Volume, Volume * progress);
            
            if (progress >= 1)
            {
                Destroy(this);
            }
        }
    }
}