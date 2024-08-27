using Game.GameEngine.Mechanics;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    public abstract class TriggerObserver :
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        protected HeroService HeroService { get; private set; }

        private IComponent_TriggerSensor heroComponent;

        [GameInject]
        public void Construct(HeroService heroService)
        {
            HeroService = heroService;
        }
        
        public virtual void InitGame()
        {
            heroComponent = HeroService.GetHero().Get<IComponent_TriggerSensor>();
        }

        public virtual void ReadyGame()
        {
            heroComponent.OnEntered += OnHeroEntered;
            heroComponent.OnExited += OnHeroExited;
        }

        public virtual void FinishGame()
        {
            heroComponent.OnEntered -= OnHeroEntered;
            heroComponent.OnExited -= OnHeroExited;
        }

        protected virtual void OnHeroEntered(Collider other)
        {
        }

        protected virtual void OnHeroExited(Collider other)
        {
        }
    }
    
    public abstract class TriggerObserver<T> : TriggerObserver where T : class
    {
        protected sealed override void OnHeroEntered(Collider other)
        {
            if (other.TryGetComponent(out T target))
            {
                OnHeroEntered(target);
            }
        }

        protected sealed override void OnHeroExited(Collider other)
        {
            if (other.TryGetComponent(out T target))
            {
                OnHeroExited(target);
            }
        }

        protected abstract void OnHeroEntered(T target);

        protected abstract void OnHeroExited(T target);
    }
}