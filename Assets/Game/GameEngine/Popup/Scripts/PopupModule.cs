using System.Collections.Generic;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class PopupModule : GameModule
    {
        [SerializeField]
        private PopupCatalog catalog;

        [SerializeField]
        private Transform container;
        
        [ShowInInspector]
        private readonly PopupManager manager = new();

        private readonly PopupSupplier supplier = new();

        private readonly PopupFactory factory = new();

        private readonly PopupInputController inputController = new();

        public override IEnumerable<IGameElement> GetElements()
        {
            yield return inputController;
        }

        public override IEnumerable<object> GetServices()
        {
            yield return manager;
        }

        public override void ConstructGame(GameContext context)
        {
            factory.Construct(catalog, container);
            supplier.Construct(context, factory);
            manager.SetSupplier(supplier);
            
            inputController.Construct(manager, context.GetService<InputStateManager>());
        }
    }
}