using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        protected virtual void OnEnable()
        {
            slider.onValueChanged.AddListener(OnVolumeChanged);
            slider.value = GetVolume();
        }

        protected virtual void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnVolumeChanged);
        }

        private void OnVolumeChanged(float volume)
        {
            SetVolume(volume);
        }

        protected abstract void SetVolume(float volume);
        
        protected abstract float GetVolume();
        
        protected void UpdateSlider(float volume)
        {
            slider.value = volume;
        }
    }
}