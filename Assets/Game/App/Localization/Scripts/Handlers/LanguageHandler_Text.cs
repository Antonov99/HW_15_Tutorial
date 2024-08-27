using System;
using LocalizationModule;
using TMPro;
using UnityEngine;

namespace Game.Localization
{
    [Serializable]
    public sealed class LanguageHandler_Text : ILanguageHandler
    {
        [TranslationKey]
        [SerializeField]
        private string key;
        
        [SerializeField]
        private TextMeshProUGUI text;

        [Space]
        [SerializeField]
        private LocalizedInt[] fontSizes = new LocalizedInt[0];

        public LanguageHandler_Text()
        {
        }

        public LanguageHandler_Text(TextMeshProUGUI text, string key)
        {
            this.text = text;
            this.key = key;
        }

        public void UpdateLanguage(SystemLanguage language)
        {
            if (text != null)
            {
                text.text = LocalizationManager.GetText(key, language);
            }
            
            if (fontSizes.FindInt(language, out var value))
            {
                text.fontSize = value;
            }
        }   
    }
}