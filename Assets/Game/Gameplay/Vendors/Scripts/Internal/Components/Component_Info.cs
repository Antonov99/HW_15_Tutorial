using Game.GameEngine.GameResources;

namespace Game.Gameplay.Vendors
{
    public sealed class Component_Info : IComponent_Info
    {
        public ResourceType ResourceType
        {
            get { return config.resourceType; }
        }

        public int PricePerOne
        {
            get { return config.pricePerOne; }
        }

        private readonly ScriptableVendor config;

        public Component_Info(ScriptableVendor config)
        {
            this.config = config;
        }
    }
}