using Elementary;
using Declarative;

namespace Game.Gameplay.Conveyors
{
    public sealed class ZoneVisualAdapter :
        IAwakeListener,
        IEnableListener,
        IDisableListener
    {
        private IVariableLimited<int> storage;

        private ZoneVisual visualZone;

        public void Construct(IVariableLimited<int> storage, ZoneVisual visualZone)
        {
            this.storage = storage;
            this.visualZone = visualZone;
        }

        void IAwakeListener.Awake()
        {
            visualZone.SetupItems(storage.Current);
        }

        void IEnableListener.OnEnable()
        {
            storage.OnValueChanged += OnItemsChanged;
        }

        void IDisableListener.OnDisable()
        {
            storage.OnValueChanged -= OnItemsChanged;
        }

        private void OnItemsChanged(int count)
        {
            visualZone.SetupItems(count);
        }
    }
}