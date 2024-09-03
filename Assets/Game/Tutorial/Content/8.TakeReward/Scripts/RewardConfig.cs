using Game.GameEngine;
using Game.Localization;
using Game.Meta;
using UnityEngine;

namespace Game.Tutorial
{
    [CreateAssetMenu(
        fileName = "Config «Reward»",
        menuName = "Tutorial/Config «Reward»"
    )]
    public sealed class RewardConfig : ScriptableObject
    {
        [Header("Quest")]
        [SerializeField]
        public MissionConfig missionConfig;
        
        [SerializeField]
        public WorldPlaceType worldPlaceType =  WorldPlaceType.BLACKSMITH;
    
        [Header("Meta")]
        [TranslationKey]
        [SerializeField]
        public string title = "Reward";

        [SerializeField]
        public Sprite icon;
    }
}