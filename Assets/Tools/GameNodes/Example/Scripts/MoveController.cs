using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using UnityEngine;

namespace GameNodes
{
    public sealed class MoveController
    {
        private IMoveInput input;

        private IComponent_MoveInDirection heroComponent;

        [GameInit]
        public void Init(IHeroService heroService, IMoveInput input)
        {
            heroComponent = heroService.GetHero().Get<IComponent_MoveInDirection>();
            this.input = input;
        }

        [GameStart]
        public void Enable()
        {
            input.OnMoved += OnDirectionMoved;
        }

        [GameFinish]
        public void Disable()
        {
            input.OnMoved -= OnDirectionMoved;
        }

        private void OnDirectionMoved(Vector2 screenDirection)
        {
            var worldDirection = new Vector3(screenDirection.x, 0.0f, screenDirection.y);
            heroComponent.Move(worldDirection);
        }
    }
}