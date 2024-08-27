using System;
using Game.GameEngine.Mechanics;
using GameSystem;
using InputModule;
using UnityEngine;

namespace Game.Gameplay.Hero
{
    [Serializable]
    public sealed class HeroMoveController : 
        IGameInitElement,
        IGameStartElement,
        IGameFinishElement
    {
        private HeroService heroService;
        
        private JoystickInput input;
        
        private IComponent_MoveInDirection heroComponent;

        [GameInject]
        public void Construct(HeroService heroService, JoystickInput input)
        {
            this.heroService = heroService;
            this.input = input;
        }
        
        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_MoveInDirection>();
        }

        void IGameStartElement.StartGame()
        {
            input.OnDirectionMoved += OnDirectionMoved;
        }

        void IGameFinishElement.FinishGame()
        {
            input.OnDirectionMoved -= OnDirectionMoved;
        }

        private void OnDirectionMoved(Vector2 screenDirection)
        {
            var worldDirection = new Vector3(screenDirection.x, 0.0f, screenDirection.y);
            heroComponent.Move(worldDirection);
        }
    }
}