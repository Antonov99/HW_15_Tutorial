using System;
using Game.Gameplay.Player;

namespace Game.Tutorial
{
    public sealed class SellResourceInspector
    {
        private SellResourceConfig config;

        private VendorInteractor sellInteractor;

        private Action callback;

        public void Construct(VendorInteractor sellInteractor, SellResourceConfig config)
        {
            this.sellInteractor = sellInteractor;
            this.config = config;
        }

        public void Inspect(Action callback)
        {
            this.callback = callback;
            sellInteractor.OnResourcesSold += OnResourcesSold;
        }

        private void OnResourcesSold(VendorSellResult result)
        {
            if (result.resourceType == config.targetResourceType)
            {
                CompleteQuest();
            }
        }

        private void CompleteQuest()
        {
            sellInteractor.OnResourcesSold -= OnResourcesSold;
            callback?.Invoke();
        }
    }
}