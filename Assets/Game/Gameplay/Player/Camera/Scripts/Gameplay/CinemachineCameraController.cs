using Cinemachine;
using Game.GameEngine.Mechanics;
using Game.Gameplay.Hero;
using GameSystem;

namespace Game.Gameplay.Player
{
    public sealed class CinemachineCameraController : 
        IGameStartElement,
        IGameFinishElement
    {
        private CinemachineVirtualCamera virtualCamera;

        private HeroService heroService;
        
        public void Construct(CinemachineVirtualCamera virtualCamera, HeroService heroService)
        {
            this.virtualCamera = virtualCamera;
            this.heroService = heroService;
        }

        void IGameStartElement.StartGame()
        {
            var hero = heroService.GetHero();
            virtualCamera.Follow = hero.Get<IComponent_GetPivot>().Pivot;
            virtualCamera.enabled = true;
        }

        void IGameFinishElement.FinishGame()
        {
            virtualCamera.enabled = false;
        }
    }
}