using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.SceneAudio
{
    public sealed class SceneAudioChannel : MonoBehaviour
    {
        private const float BLOCKED_AUDIO_DELAY = 0.1f;

        public bool IsEnabled
        {
            get { return isEnable; }
            set { SetEnable(value); }
        }

        public float Volume
        {
            get { return volume; }
            set { SetVolume(value); }
        }

        [SerializeField]
        private bool isEnable;

        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volume;

        [Space]
        [SerializeField]
        private AudioSource source;

        [SerializeField]
        private bool controlClips;
        
        private readonly List<BlockedAudio> blockedClips = new();
        private readonly List<BlockedAudio> cache = new();

        private readonly List<ISceneAudioListener> listeners = new();

        public void PlaySound(AudioClip clip)
        {
            if (!isEnable)
            {
                return;
            }

            var clipName = clip.name;
            
            if (controlClips)
            {
                if (IsBlocked(clipName))
                {
                    return;
                }

                blockedClips.Add(new BlockedAudio(clipName, BLOCKED_AUDIO_DELAY));
            }

            source.PlayOneShot(clip);
        }

        private void SetEnable(bool enable)
        {
            if (isEnable == enable)
            {
                return;
            }

            isEnable = enable;
            source.enabled = enable;

            for (int i = 0, count = listeners.Count; i < count; i++)
            {
                var observer = listeners[i];
                observer.OnEnabled(enable);
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
            source.volume = volume;

            for (int i = 0, count = listeners.Count; i < count; i++)
            {
                var observer = listeners[i];
                observer.OnVolumeChanged(volume);
            }
        }

        public void AddListener(ISceneAudioListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(ISceneAudioListener listener)
        {
            listeners.Remove(listener);
        }

        private void Awake()
        {
            source.enabled = isEnable;
            source.volume = volume;
        }

        private void Update()
        {
            if (isEnable)
            {
                ProcessBlockedClips(Time.deltaTime);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                Awake();
            }
            catch (Exception)
            {
            }
        }
#endif

        private bool IsBlocked(string soundName)
        {
            for (int i = 0, count = blockedClips.Count; i < count; i++)
            {
                var clip = blockedClips[i];
                if (clip.name == soundName)
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessBlockedClips(float deltaTime)
        {
            if (!controlClips)
            {
                return;
            }
            
            cache.Clear();
            cache.AddRange(blockedClips);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var clip = cache[i];
                var remainingTime = clip.delay - deltaTime;

                if (remainingTime <= 0)
                {
                    blockedClips.RemoveAt(i);
                }
                else
                {
                    blockedClips[i] = new BlockedAudio(clip.name, remainingTime);
                }
            }
        }

        private readonly struct BlockedAudio
        {
            public readonly string name;
            public readonly float delay;

            public BlockedAudio(string name, float delay)
            {
                this.name = name;
                this.delay = delay;
            }
        }
    }
}