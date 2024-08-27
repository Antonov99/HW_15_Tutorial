using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Tutorial.Gameplay
{
    public sealed class NavigationManager : MonoBehaviour,
        IGameConstructElement,
        IGameInitElement
    {
        [SerializeField]
        private NavigationArrow arrow;

        private IComponent_GetPosition heroComponent;

        private IHeroService heroService;

        [PropertySpace]
        [ReadOnly]
        [ShowInInspector]
        private Vector3 targetPosition;

        [ReadOnly]
        [ShowInInspector]
        private bool isActive;

        private void Awake()
        {
            arrow.Hide();
        }

        private void Update()
        {
            if (isActive)
            {
                arrow.SetPosition(heroComponent.Position);
                arrow.LookAt(targetPosition);
            }
        }
        
        [Button]
        public void StartLookAt(Transform targetPoint)
        {
            StartLookAt(targetPoint.position);
        }

        public void StartLookAt(Vector3 targetPosition)
        {
            arrow.Show();
            isActive = true;
            this.targetPosition = targetPosition;
        }

        public void Stop()
        {
            arrow.Hide();
            isActive = false;
        }

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            heroService = context.GetService<HeroService>();
        }

        void IGameInitElement.InitGame()
        {
            heroComponent = heroService.GetHero().Get<IComponent_GetPosition>();
        }
    }
}