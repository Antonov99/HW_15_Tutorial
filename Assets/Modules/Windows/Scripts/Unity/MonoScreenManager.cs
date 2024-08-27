using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Windows
{
    public abstract class MonoScreenManager<T> : MonoBehaviour, IScreenManager<T>
    {
        public event Action<T> OnScreenShown;

        public event Action<T> OnScrenHidden;

        public event Action<T> OnScreenChanged;

        protected abstract IWindowSupplier<T, MonoWindow> Supplier { get; }

        private IScreenManager<T> manager;

        protected virtual void Awake()
        {
            manager = new ScreenManager<T, MonoWindow>(Supplier);
        }

        protected virtual void OnEnable()
        {
            manager.OnScreenShown += OnShowScreen;
            manager.OnScreenChanged += OnChangeScreen;
            manager.OnScrenHidden += OnHideScreen;
        }

        protected virtual void OnDisable()
        {
            manager.OnScreenShown -= OnShowScreen;
            manager.OnScreenChanged -= OnChangeScreen;
            manager.OnScrenHidden -= OnHideScreen;
        }

        [Button]
        public void ChangeScreen(T key, object args = default)
        {
            manager.ChangeScreen(key, args);
        }

        public bool IsScreenActive(T key)
        {
            return manager.IsScreenActive(key);
        }

        private void OnShowScreen(T screenName)
        {
            OnScreenShown?.Invoke(screenName);
        }

        private void OnHideScreen(T screenName)
        {
            OnScrenHidden?.Invoke(screenName);
        }

        private void OnChangeScreen(T screenName)
        {
            OnScreenChanged?.Invoke(screenName);
        }
    }
}