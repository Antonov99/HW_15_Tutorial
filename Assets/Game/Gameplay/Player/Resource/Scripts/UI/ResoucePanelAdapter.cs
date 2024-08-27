using Game.GameEngine.GameResources;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class ResoucePanelAdapter : MonoBehaviour,
        IGameConstructElement,
        IGameInitElement,
        IGameReadyElement,
        IGameFinishElement
    {
        [SerializeField]
        private bool listenIncome = false;

        [SerializeField]
        private bool listenSpend = true;

        [SerializeField]
        private ResourcePanel panel;

        private ResourceStorage resourceStorage;

        void IGameConstructElement.ConstructGame(GameContext context)
        {
            resourceStorage = context.GetService<ResourceStorage>();
        }

        void IGameInitElement.InitGame()
        {
            SetupPanel();
        }

        void IGameReadyElement.ReadyGame()
        {
            if (listenIncome)
            {
                resourceStorage.OnResourceAdded += OnResourceAdded;
            }

            if (listenSpend)
            {
                resourceStorage.OnResourceExtracted += OnResourceExtracted;
            }
        }

        void IGameFinishElement.FinishGame()
        {
            resourceStorage.OnResourceExtracted -= OnResourceExtracted;
            resourceStorage.OnResourceAdded -= OnResourceAdded;
        }

        private void SetupPanel()
        {
            var resources = resourceStorage.GetAllResources();
            for (var i = 0; i < resources.Length; i++)
            {
                var resource = resources[i];
                panel.SetupItem(resource.type, resource.amount);
            }
        }

        private void OnResourceAdded(ResourceType type, int range)
        {
            panel.IncrementItem(type, range);
        }

        private void OnResourceExtracted(ResourceType type, int range)
        {
            panel.DecrementItem(type, range);
        }

#if UNITY_EDITOR

        [Title("Debug")]
        [Button("Sync Resources")]
        private void Editor_SyncResources()
        {
            SetupPanel();
        }
#endif
    }
}