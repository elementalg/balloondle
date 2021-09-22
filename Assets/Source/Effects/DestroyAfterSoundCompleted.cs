using System;
using UnityEngine;

namespace Balloondle.Effects
{
    public class DestroyAfterSoundCompleted : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void OnEnable()
        {
            if (GetComponent<AudioSource>() == null)
            {
                throw new InvalidOperationException("Missing AudioSource component.");
            }
            
            _audioSource = GetComponent<AudioSource>();
        }

        private void LateUpdate()
        {
            if (!_audioSource.isPlaying && !_audioSource.loop)
            {
                Destroy(gameObject, 0f);
            }
        }
    }
}