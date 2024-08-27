using GameSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Meta
{
    public sealed class BoosterListViewAdapter : MonoBehaviour,
        IGameConstructElement,
        IGameReadyElement,
        IGameFinishElement
    {
        [SerializeField]
        [FormerlySerializedAs("viewList")]
        private BoosterListView listView;

        private BoostersManager boostersManager;

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            boostersManager = context.GetService<BoostersManager>();
        }

        void IGameReadyElement.ReadyGame()
        {
            boostersManager.OnBoosterStarted += OnBoosterStarted;
            boostersManager.OnBoosterFinished += OnBoosterFinished;
        }

        void IGameFinishElement.FinishGame()
        {
            boostersManager.OnBoosterStarted -= OnBoosterStarted;
            boostersManager.OnBoosterFinished -= OnBoosterFinished;
        }

        private void OnBoosterStarted(Booster booster)
        {
            listView.AddBooster(booster);
        }

        private void OnBoosterFinished(Booster booster)
        {
            listView.RemoveBooster(booster);
        }
    }
}