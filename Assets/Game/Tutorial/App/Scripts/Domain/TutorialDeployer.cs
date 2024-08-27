using System.Threading.Tasks;
using Game.App;
using Game.GameEngine.GUI;
using GameSystem;
using Services;
using UnityEngine;

namespace Game.Tutorial.App
{
    public sealed class TutorialDeployer : IAppInitListener
    {
        private const string ENGINE_NAME = "Tutorial";

        private TutorialAssetSupplier assetSupplier;

        private GameFacade gameFacade;

        void IAppInitListener.Init()
        {
            assetSupplier = ServiceLocator.GetService<TutorialAssetSupplier>();
            gameFacade = ServiceLocator.GetService<GameFacade>();
        }

        public async Task DeployTutorial()
        {
            var manager = TutorialManager.Instance;
            if (manager.IsCompleted)
            {
                return;
            }

            //Load tutorial engine:
            var enginePrefab = await assetSupplier.LoadTutorialEngine();
            var engine = Object.Instantiate(enginePrefab);
            engine.name = ENGINE_NAME;
            
            //Load tutorial gui:
            var guiPrefab = await assetSupplier.LoadTutorialInterface();
            var canvasService = gameFacade.GetService<GUICanvasService>();
            var gui = Object.Instantiate(guiPrefab, canvasService.RootTransform);
            gui.name = guiPrefab.name;
            
            //Register services:
            gameFacade.RegisterService(engine.GetComponent<IGameServiceGroup>());
            gameFacade.RegisterService(gui.GetComponent<IGameServiceGroup>());

            //Register elements:
            gameFacade.RegisterElement(engine.GetComponent<IGameElementGroup>());
            gameFacade.RegisterElement(gui.GetComponent<IGameElementGroup>());
        }
    }
}