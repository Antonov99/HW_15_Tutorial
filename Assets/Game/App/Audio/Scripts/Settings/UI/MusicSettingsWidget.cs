using Game.UI;
using Services;

namespace Game.App
{
    public sealed class MusicSettingsWidget : AudioSettingsWidget
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            AudioSettingsManager.OnMusicVolumeChangd += UpdateSlider;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            AudioSettingsManager.OnMusicVolumeChangd -= UpdateSlider;
        }

        protected override void SetVolume(float volume)
        {
            AudioSettingsManager.SetMusicVolume(volume);
        }

        protected override float GetVolume()
        {
            return AudioSettingsManager.MusicVolume;
        }
    }
}