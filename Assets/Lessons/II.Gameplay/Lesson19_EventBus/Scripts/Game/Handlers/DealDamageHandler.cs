using System;
using JetBrains.Annotations;
using Lessons.Entities.Common.Components;
using Lessons.Game.Events;
using VContainer.Unity;

namespace Lessons.Game.Handlers
{
    [UsedImplicitly]
    public class DealDamageHandler : IInitializable, IDisposable
    {
        private readonly EventBus _eventBus;

        public DealDamageHandler(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        void IInitializable.Initialize()
        {
            _eventBus.Subscribe<DealDamageEvent>(OnDealDamage);
        }

        void IDisposable.Dispose()
        {
            _eventBus.Unsubscribe<DealDamageEvent>(OnDealDamage);
        }

        private void OnDealDamage(DealDamageEvent evt)
        {
            if (!evt.Entity.TryGet(out HitPointsComponent hitPointsComponent))
            {
                return;
            }

            hitPointsComponent.Value -= evt.Strength;

            if (hitPointsComponent.Value <= 0)
            {
                _eventBus.RaiseEvent(new DestroyEvent(evt.Entity));
            }
        }
    }
}