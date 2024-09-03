using Game.GameEngine.GameResources;
using Game.Localization;
using UnityEngine;

namespace Game.Tutorial
{
    [CreateAssetMenu(
        fileName = "Config «Get Resource»",
        menuName = "Tutorial/New Config «Get Resource»"
    )]
    public sealed class GetResourceConfig : ScriptableObject
    {
        [Header("Quest")]
        
        [Header("Meta")]
        [TranslationKey]
        [SerializeField]
        public string title = "Get Lumber";

        [SerializeField]
        public Sprite icon;
    }
}