using System;
using UnityEngine;

namespace Game.Meta
{
    [Serializable]
    public sealed class Component_BoosterInfo : IComponent_BoosterInfo
    {
        public BoosterConfig BoosterInfo
        {
            get { return config; }
        }

        [SerializeField]
        private BoosterConfig config;
    }
}