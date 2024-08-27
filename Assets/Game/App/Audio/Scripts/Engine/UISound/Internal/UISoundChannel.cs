using System;
using UnityEngine;

namespace Game
{
    public sealed class UISoundChannel : MonoBehaviour
    {
        public event Action<bool> OnEnabled;
        public event Action<float> OnVolumeChanged;

        public bool IsEnable
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

        [Range(0, 1)]
        [SerializeField]
        private float volume;

        [Space]
        [SerializeField]
        private AudioSource source;

        public void PlaySound(AudioClip clip)
        {
            if (isEnable)
            {
                source.PlayOneShot(clip);
            }
        }

        private void Awake()
        {
            source.volume = volume;
            source.enabled = isEnable;
        }

        private void SetEnable(bool enable)
        {
            if (isEnable == enable)
            {
                return;
            }

            isEnable = enable;
            source.enabled = enable;
            OnEnabled?.Invoke(enable);
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
            OnVolumeChanged?.Invoke(volume);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                source.volume = volume;
                source.enabled = isEnable;
            }
            catch (Exception)
            {
            }
        }
#endif
    }
}