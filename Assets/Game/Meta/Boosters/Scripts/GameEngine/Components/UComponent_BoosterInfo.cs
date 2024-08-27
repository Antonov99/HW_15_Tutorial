using UnityEngine;

namespace Game.Meta
{
    public sealed class UComponent_BoosterInfo : MonoBehaviour, IComponent_BoosterInfo
    {
        public BoosterConfig BoosterInfo
        {
            get { return config; }
        }

        [SerializeField]
        private BoosterConfig config;
    }
}