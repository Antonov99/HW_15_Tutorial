using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    public sealed class BoostersManager : 
        IGameStartElement,
        IGameFinishElement
    {
        public event Action<Booster> OnBoosterLaunched;

        public event Action<Booster> OnBoosterStarted;

        public event Action<Booster> OnBoosterFinished;
        
        private BoosterFactory factory;

        private MonoBehaviour monoContext;

        [PropertySpace(8), ReadOnly, ShowInInspector]
        private readonly List<Booster> currentBoosters = new();

        [GameInject]
        public void Construct(MonoBehaviour monoContext, BoosterFactory factory)
        {
            this.factory = factory;
            this.monoContext = monoContext;
        }

        [Title("Methods")]
        [Button]
        [GUIColor(0, 1, 0)]
        public void LaunchBooster(BoosterConfig config)
        {
            var booster = factory.CreateBooster(config);
            booster.OnEnded += OnEndBooster;

            currentBoosters.Add(booster);

            booster.Start();
            OnBoosterStarted?.Invoke(booster);
            OnBoosterLaunched?.Invoke(booster);
        }

        public Booster SetupBooster(BoosterConfig config)
        {
            var booster = factory.CreateBooster(config);
            booster.OnEnded += OnEndBooster;
            
            currentBoosters.Add(booster);
            return booster;
        }

        public Booster[] GetActiveBoosters()
        {
            return currentBoosters.ToArray();
        }

        void IGameStartElement.StartGame()
        {
            StartAllBoosters();
        }

        void IGameFinishElement.FinishGame()
        {
            StopAllBoosters();
        }

        private void StartAllBoosters()
        {
            for (int i = 0, count = currentBoosters.Count; i < count; i++)
            {
                var booster = currentBoosters[i];
                if (booster.IsActive)
                {
                    continue;
                }

                booster.Start();
                OnBoosterStarted?.Invoke(booster);
            }
        }

        private void StopAllBoosters()
        {
            for (int i = 0, count = currentBoosters.Count; i < count; i++)
            {
                var booster = currentBoosters[i];
                if (!booster.IsActive)
                {
                    continue;
                }

                booster.OnEnded -= OnEndBooster;
                booster.Stop();
            }
        }

        private void OnEndBooster(Booster booster)
        {
            booster.OnEnded -= OnEndBooster;
            monoContext.StartCoroutine(EndBoosterInNextFrame(booster));
        }

        private IEnumerator EndBoosterInNextFrame(Booster booster)
        {
            yield return new WaitForEndOfFrame();
            currentBoosters.Remove(booster);
            OnBoosterFinished?.Invoke(booster);
        }
    }
}