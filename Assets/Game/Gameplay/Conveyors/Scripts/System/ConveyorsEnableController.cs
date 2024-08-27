using Game.GameEngine.Mechanics;
using GameSystem;
using UnityEngine;

namespace Game.Gameplay.Conveyors
{
    public sealed class ConveyorsEnableController : 
        IGameStartElement,
        IGameFinishElement
    {
        private ConveyorsService conveyorsService;

        [GameInject]
        public void Construct(ConveyorsService conveyorsService)
        {
            this.conveyorsService = conveyorsService;
        }

        void IGameStartElement.StartGame()
        {
            EnableConveyors(true);
        }

        void IGameFinishElement.FinishGame()
        {
            EnableConveyors(false);
        }

        private void EnableConveyors(bool isEnable)
        {
            var conveyors = conveyorsService.GetAllConveyors();
            foreach (var conveyor in conveyors)
            {
                var enableComponent = conveyor.Get<IComponent_Enable>();
                enableComponent.SetEnable(isEnable);
            }
        }
    }
}