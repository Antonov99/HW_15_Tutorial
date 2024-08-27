using Game.App;
using LocalizationModule;
using UnityEngine;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Game.Localization
{
    public class LocalizationComponent : MonoBehaviour
    {
        [SerializeReference]
        private ILanguageHandler[] children = new ILanguageHandler[0];

        protected virtual void OnEnable()
        {
            UpdateLanguage(LanguageManager.CurrentLanguage);
            LanguageManager.OnLanguageChanged += UpdateLanguage;
        }

        protected virtual void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= UpdateLanguage;
        }

        protected virtual void UpdateLanguage(SystemLanguage language)
        {
            for (int i = 0, count = children.Length; i < count; i++)
            {
                var handler = children[i];
                handler.UpdateLanguage(language);
            }
        }
    }
}