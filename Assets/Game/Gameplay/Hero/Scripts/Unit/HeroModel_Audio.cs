using System;
using Game.GameEngine;
using Declarative;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class HeroModel_Audio
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private SoundCatalog soundCatalog;

        [Space]
        [SerializeField]
        private string humanDeathSoundId = "HumanDeath";

        [ShowInInspector, ReadOnly]
        public SoundEmitter soundEmitter;

        [Construct]
        private void ConstructEmiiter()
        {
            soundEmitter = new SoundEmitter(audioSource, soundCatalog);
        }

        [Construct]
        private void ConstructDeath(HeroModel_Core core)
        {
            core.life.deathEmitter.AddListener(_ => soundEmitter.PlaySound(humanDeathSoundId));
        }
    }
}