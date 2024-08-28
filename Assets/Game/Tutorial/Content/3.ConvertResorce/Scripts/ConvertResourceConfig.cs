using Game.GameEngine.GameResources;
using Game.Localization;
using UnityEngine;

namespace Game.Tutorial
{
    [CreateAssetMenu(
        fileName = "Config «Convert Resource»",
        menuName = "Tutorial/New Config «Convert Resource»"
    )]
    public sealed class ConvertResourceConfig : ScriptableObject
    {
        [Header("Quest")]
        
        [Header("Meta")]
        [TranslationKey]
        [SerializeField]
        public string title = "PUT TREE";

        [SerializeField]
        public Sprite icon;
    }
}