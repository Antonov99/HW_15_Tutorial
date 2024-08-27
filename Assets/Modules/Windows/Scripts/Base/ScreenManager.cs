using System;
using Sirenix.OdinInspector;

namespace Windows
{
    public sealed class ScreenManager<TKey, TScreen> : IScreenManager<TKey> where TScreen : IWindow
    {
        public event Action<TKey> OnScreenShown;

        public event Action<TKey> OnScrenHidden;

        public event Action<TKey> OnScreenChanged;

        private IWindowSupplier<TKey, TScreen> supplier;

        private TKey currentScreenKey;

        private TScreen currentScreen;

        public ScreenManager(IWindowSupplier<TKey, TScreen> supplier = null)
        {
            this.supplier = supplier;
        }

        public bool IsScreenActive(TKey key)
        {
            return ReferenceEquals(currentScreenKey, key);
        }

        [Button]
        public void ChangeScreen(TKey key, object args = default)
        {
            if (!ReferenceEquals(currentScreen, null))
            {
                HideScreenInternal(currentScreenKey, currentScreen);
            }

            currentScreenKey = key;
            ShowScreenInternal(key, args);
            OnScreenChanged?.Invoke(key);
        }
        
        private void ShowScreenInternal(TKey key, object args)
        {
            currentScreen = supplier.LoadWindow(key);
            currentScreen.Show(args);
            OnScreenShown?.Invoke(key);
        }

        private void HideScreenInternal(TKey key, TScreen screen)
        {
            screen.Hide();
            supplier.UnloadWindow(screen);
            OnScrenHidden?.Invoke(key);
        }
        
        public void SetSupplier(IWindowSupplier<TKey, TScreen> supplier)
        {
            this.supplier = supplier;
        }
    }
}