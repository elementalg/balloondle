using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Balloondle.Ambient
{
    /// <summary>
    /// Basic controller of sounds with incorporated transitions through cross fading.
    /// </summary>
    public class AmbientPlayer : MonoBehaviour
    {
        [SerializeField, Tooltip("Asset which defines the available ambient sounds.")]
        private AmbientPlaylistsDefinition m_AmbientPlaylistsDefinition;

        [SerializeField] 
        private float m_CrossFadeDuration = 0.5f;

        private List<AmbientSound> _ambientSounds;
        private int _currentSoundIndex;

        private AudioSource _audioSource;
        private float _volume = 1f;

        public float Volume
        {
            get => _volume;
            set
            {
                if (_audioSource != null)
                {
                    _audioSource.volume = value;
                }

                _volume = value;
            }
        }

        private void OnEnable()
        {
            _ambientSounds = new List<AmbientSound>();
        }

        /// <summary>
        /// Starts playing the passed playlist.
        /// </summary>
        /// <param name="playlistName"></param>
        /// <param name="random">whether or not the playlist shall be played in sequential order.</param>
        /// <exception cref="ArgumentException">if the playlist could not be found.</exception>
        public void Play(string playlistName, bool random = false)
        {
            AmbientPlaylist playlist = GetPlaylistByName(playlistName) ?? 
                                       throw new ArgumentException($"Cannot find playlist '{playlistName}'.");

            _currentSoundIndex = 0;
            _ambientSounds.Clear();
            
            if (random)
            {
                FillAmbientSoundsWithPlaylistRandomly(playlist);
            }
            else
            {
                FillAmbientSoundsWithPlaylistInSequentialOrder(playlist);
            }

            if (_audioSource != null)
            {
                SoundFadeOut fadeOut = _audioSource.gameObject.AddComponent<SoundFadeOut>();
                fadeOut.FadeOutDuration = m_CrossFadeDuration;
            }

            GameObject audioPlayer = new GameObject
            {
                transform =
                {
                    parent = transform
                }
            };

            _audioSource = audioPlayer.AddComponent<AudioSource>();
            PlayNextSound();
            
            SoundFadeIn fadeIn = _audioSource.gameObject.AddComponent<SoundFadeIn>();
            fadeIn.Volume = Volume;
            fadeIn.FadeInDuration = m_CrossFadeDuration;
        }

        #nullable enable
        private AmbientPlaylist? GetPlaylistByName(string playlistName)
        {
            foreach (AmbientPlaylist playlist in m_AmbientPlaylistsDefinition.m_AmbientPlaylists)
            {
                if (playlist.m_Name.Equals(playlistName))
                {
                    return playlist;
                }
            }

            return null;
        }
        #nullable disable

        private void FillAmbientSoundsWithPlaylistRandomly(AmbientPlaylist playlist)
        {
            List<int> addedIndexes = new List<int>();

            while (addedIndexes.Count < playlist.m_Sounds.Count)
            {
                int randomIndex = Random.Range(0, playlist.m_Sounds.Count);

                // Avoid repeated indexes.
                if (!addedIndexes.Contains(randomIndex))
                {
                    addedIndexes.Add(randomIndex);
                    _ambientSounds.Add(playlist.m_Sounds[randomIndex]);
                }
            }
            
            addedIndexes.Clear();
        }
        
        private void FillAmbientSoundsWithPlaylistInSequentialOrder(AmbientPlaylist playlist)
        {
            for (int i = 0; i < playlist.m_Sounds.Count; i++)
            {
                _ambientSounds.Add(playlist.m_Sounds[i]);
            }
        }

        private void PlayNextSound()
        {
            _audioSource.clip = _ambientSounds[_currentSoundIndex].m_Sound;
            _audioSource.Play();
        }

        private void Update()
        {
            if (_audioSource != null)
            {
                if (_audioSource.isPlaying)
                {
                    return;
                }

                _currentSoundIndex = (_currentSoundIndex + 1 > _ambientSounds.Count) ? 0 : _currentSoundIndex + 1;
                PlayNextSound();
            }
        }

        /// <summary>
        /// Stops any sound being played by fading it out.
        /// </summary>
        public void Stop()
        {
            SoundFadeOut fadeOut = _audioSource.gameObject.AddComponent<SoundFadeOut>();
            fadeOut.FadeOutDuration = m_CrossFadeDuration;

            _audioSource = null;
            _ambientSounds.Clear();
        }
    }
}