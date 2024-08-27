using System;
using Elementary;
using Game.GameEngine;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Player;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class WorldPlaceVisitor : TriggerVisitor<WorldPlaceTrigger>
    {
        [Space]
        [SerializeField]
        private float visitDelay = 0.2f;

        private WorldPlaceVisitInteractor visitInteractor;

        [GameInject]
        public void Construct(WorldPlaceVisitInteractor placeObservable)
        {
            visitInteractor = placeObservable;
        }

        protected override bool CanEnter(WorldPlaceTrigger entity)
        {
            return true;
        }

        protected override ICondition ProvideConditions(WorldPlaceTrigger target)
        {
            return new ConditionComposite(
                new ConditionCountdown(monoContext, seconds: visitDelay, startInstantly: true),
                new Condition_Entity_IsNotMoving(HeroService.GetHero())
            );
        }

        protected override void OnHeroVisit(WorldPlaceTrigger target)
        {
            var placeType = target.PlaceType;
            if (visitInteractor.IsVisiting && visitInteractor.CurrentPlace != placeType)
            {
                visitInteractor.EndVisit();
            }

            visitInteractor.StartVisit(placeType);
        }

        protected override void OnHeroQuit(WorldPlaceTrigger target)
        {
            var placeType = target.PlaceType;
            if (visitInteractor.IsVisiting && placeType == target.PlaceType)
            {
                visitInteractor.EndVisit();
            }
        }
    }
}