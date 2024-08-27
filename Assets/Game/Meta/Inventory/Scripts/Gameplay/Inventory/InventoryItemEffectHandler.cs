using Game.GameEngine.InventorySystem;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;
using Services;
using UnityEngine;

namespace Game.Meta
{
    public sealed class InventoryItemEffectHandler : InventoryObserver,
        IGameInitElement,
        IGameStartElement
    {
        private HeroService heroService;

        private IComponent_Effector heroComponent;

        public void Construct(HeroService heroService)
        {
            this.heroService = heroService;
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_Effector>();
        }

        void IGameStartElement.StartGame()
        {
            ActivateExisitingEffects();
        }

        protected override void OnItemAdded(InventoryItem item)
        {
            if (item.FlagsExists(InventoryItemFlags.EFFECTIBLE))
            {
                ActivateEffect(item);
            }
        }

        protected override void OnItemRemoved(InventoryItem item)
        {
            if (item.FlagsExists(InventoryItemFlags.EFFECTIBLE))
            {
                DeactivateEffect(item);
            }
        }

        private void ActivateEffect(InventoryItem item)
        {
            var effect = item.GetComponent<IComponent_GetEffect>().Effect;
            heroComponent.Apply(effect);
        }

        private void DeactivateEffect(InventoryItem item)
        {
            var effect = item.GetComponent<IComponent_GetEffect>().Effect;
            heroComponent.Discard(effect);
        }

        private void ActivateExisitingEffects()
        {
            var items = inventory.FindItems(InventoryItemFlags.EFFECTIBLE);
            for (int i = 0, count = items.Length; i < count; i++)
            {
                var item = items[i];
                ActivateEffect(item);
            }
        }
    }
}