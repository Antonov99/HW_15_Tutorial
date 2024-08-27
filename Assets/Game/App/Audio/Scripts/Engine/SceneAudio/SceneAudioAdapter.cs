using UnityEngine;
using UnityEngine.Serialization;

namespace Game.SceneAudio
{
    public sealed class SceneAudioAdapter : MonoBehaviour, ISceneAudioListener
    {
        [FormerlySerializedAs("channelType")]
        [SerializeField]
        private SceneAudioType audioType;

        [Space]
        [SerializeField]
        private AudioSource[] audioSources;

        private void OnEnable()
        {
            if (SceneAudioManager.IsInitialized)
            {
                Initialize();
            }
            else
            {
                SceneAudioManager.OnInitialized += Initialize;
            }
        }

        private void OnDisable()
        {
            SceneAudioManager.RemoveListener(audioType, this);
        }

        private void Initialize()
        {
            SceneAudioManager.OnInitialized -= Initialize;
            SceneAudioManager.AddListener(audioType, this);
            SetEnable(SceneAudioManager.IsEnable(audioType));
            SetVolume(SceneAudioManager.GetVolume(audioType));
        }

        void ISceneAudioListener.OnEnabled(bool enabled)
        {
            SetEnable(enabled);
        }

        void ISceneAudioListener.OnVolumeChanged(float volume)
        {
            SetVolume(volume);
        }
        
        private void SetEnable(bool enabled)
        {
            for (int i = 0, count = audioSources.Length; i < count; i++)
            {
                var source = audioSources[i];
                source.enabled = enabled;
            }
        }

        private void SetVolume(float volume)
        {
            for (int i = 0, count = audioSources.Length; i < count; i++)
            {
                var source = audioSources[i];
                source.volume = volume;
            }
        }
    }
}