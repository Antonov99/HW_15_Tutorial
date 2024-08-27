using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public sealed class MusicPlayer : MonoBehaviour
    {
        public event Action<bool> OnMuted;
        public event Action<float> OnVolumeChanged;
        public event Action OnStarted;
        public event Action OnPaused;
        public event Action OnResumed;
        public event Action OnStopped;
        public event Action OnFinsihed;

        public bool IsMute
        {
            get { return isMute; }
            set { SetMute(value); }
        }

        public float Volume
        {
            get { return audioSource.volume; }
            set { SetVolume(value); }
        }

        [PropertySpace(8.0f)]
        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(-8)]
        public MusicState State
        {
            get { return state; }
        }

        [PropertyOrder(-7)]
        [ReadOnly]
        [ShowInInspector]
        public AudioClip CurrentMusic
        {
            get { return GetCurrentMusic(); }
        }

        [PropertyOrder(-6)]
        [ReadOnly]
        [ShowInInspector]
        [ProgressBar(min: 0, max: 1, r: 1f, g: 0.83f, b: 0f)]
        public float PlayingProgress
        {
            get { return GetPlayingProgress(); }
        }

        [PropertySpace(8.0f)]
        [PropertyOrder(-10)]
        [SerializeField]
        private bool isMute;

        [PropertyOrder(-9)]
        [Range(0, 1.0f)]
        [SerializeField]
        private float volume;
        
        private MusicState state;

        [PropertySpace(8.0f)]
        [PropertyOrder(-2)]
        [SerializeField]
        private AudioSource audioSource;
        
        [PropertySpace(8.0f)]
        [SerializeField]
        private bool randomizePitch = true;

        [SerializeField, ShowIf(nameof(randomizePitch))]
        private float pitchOffset = 0.2f;

        [Title("Methods")]
        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Play(AudioClip music)
        {
            if (state != MusicState.IDLE)
            {
                Debug.LogWarning("Music is already started!");
                return;
            }

            state = MusicState.PLAYING;
            
            if (randomizePitch)
            {
                audioSource.pitch = Random.Range(1 - pitchOffset, 1 + pitchOffset);
            }
            
            audioSource.clip = music;
            audioSource.Play();
            OnStarted?.Invoke();
        }

        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Pause()
        {
            if (state != MusicState.PLAYING)
            {
                Debug.LogWarning("Music is not playing!");
                return;
            }

            state = MusicState.PAUSED;
            audioSource.Pause();
            OnPaused?.Invoke();
        }

        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Resume()
        {
            if (state != MusicState.PAUSED)
            {
                Debug.LogWarning("Music is not paused!");
                return;
            }

            state = MusicState.PLAYING;
            audioSource.UnPause();
            OnResumed?.Invoke();
        }

        [GUIColor(1f, 0.83f, 0f)]
        [Button]
        public void Stop()
        {
            if (state == MusicState.IDLE)
            {
                Debug.LogWarning("Music is not playing!");
                return;
            }

            state = MusicState.IDLE;
            audioSource.Stop();
            audioSource.clip = null;
            OnStopped?.Invoke();
        }

        private void Finish()
        {
            state = MusicState.IDLE;
            audioSource.Stop();
            audioSource.clip = null;
            OnFinsihed?.Invoke();
        }

        private void Awake()
        {
            audioSource.volume = volume;
            audioSource.mute = isMute;
            state = MusicState.IDLE;
        }

        private void Update()
        {
            if (state == MusicState.PLAYING && audioSource.time >= audioSource.clip.length)
            {
                Finish();
            }
        }

        private void SetVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            if (Mathf.Approximately(volume, this.volume))
            {
                return;
            }

            this.volume = volume;
            audioSource.volume = volume;
            OnVolumeChanged?.Invoke(volume);
        }

        private void SetMute(bool mute)
        {
            if (isMute == mute)
            {
                return;
            }

            isMute = mute;
            audioSource.mute = mute;
            OnMuted?.Invoke(mute);
        }

        private float GetPlayingProgress()
        {
            if (state == MusicState.IDLE)
            {
                return 0.0f;
            }

            if (audioSource == null || audioSource.clip == null)
            {
                return 0.0f;
            }

            return audioSource.time / audioSource.clip.length;
        }
        
        private AudioClip GetCurrentMusic()
        {
            if (audioSource != null)
            {
                return audioSource.clip;
            }

            return null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                audioSource.volume = volume;
                audioSource.mute = isMute;
            }
            catch (Exception)
            {
            }
        }
#endif
    }
}