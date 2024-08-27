using UnityEngine;

namespace Windows.Examples
{
    public sealed class MyScreenManager : MonoScreenManager<MyScreenId>
    {
        protected override IWindowSupplier<MyScreenId, MonoWindow> Supplier
        {
            get { return supplier; }
        }

        [SerializeField]
        private MyScreenSupplier supplier;
    }
}