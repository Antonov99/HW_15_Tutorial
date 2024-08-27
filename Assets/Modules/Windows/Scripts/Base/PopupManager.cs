using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Windows
{
    public class PopupManager<TKey, TPopup> : IPopupManager<TKey>, IWindow.Callback where TPopup : IWindow
    {
        public event Action<TKey> OnPopupShown;

        public event Action<TKey> OnPopupHidden;

        public bool HasActivePopups
        {
            get { return activePopups.Count > 0; }
        }

        private IWindowSupplier<TKey, TPopup> supplier;

        private readonly Dictionary<TKey, TPopup> activePopups;

        private readonly List<TKey> cache;

        public PopupManager(IWindowSupplier<TKey, TPopup> supplier = null)
        {
            this.supplier = supplier;
            activePopups = new Dictionary<TKey, TPopup>();
            cache = new List<TKey>();
        }

        [Button]
        public void ShowPopup(TKey key, object args = default)
        {
            if (!IsPopupActive(key))
            {
                ShowPopupInternal(key, args);
            }
        }

        [Button]
        public void HidePopup(TKey key)
        {
            if (IsPopupActive(key))
            {
                HidePopupInternal(key);
            }
        }

        [Button]
        public void HideAllPopups()
        {
            cache.Clear();
            cache.AddRange(activePopups.Keys);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var popupName = cache[i];
                HidePopupInternal(popupName);
            }
        }

        public bool IsPopupActive(TKey key)
        {
            return activePopups.ContainsKey(key);
        }

        void IWindow.Callback.OnClose(IWindow window)
        {
            var popup = (TPopup) window;
            if (TryFindName(popup, out var popupName))
            {
                HidePopup(popupName);
            }
        }

        private void ShowPopupInternal(TKey name, object args)
        {
            var popup = supplier.LoadWindow(name);
            popup.Show(args, callback: this);

            activePopups.Add(name, popup);
            OnPopupShown?.Invoke(name);
        }

        private void HidePopupInternal(TKey name)
        {
            var popup = activePopups[name];
            popup.Hide();

            activePopups.Remove(name);
            supplier.UnloadWindow(popup);
            OnPopupHidden?.Invoke(name);
        }

        private bool TryFindName(TPopup popup, out TKey name)
        {
            foreach (var (key, otherPopup) in activePopups)
            {
                if (ReferenceEquals(popup, otherPopup))
                {
                    name = key;
                    return true;
                }
            }

            name = default;
            return false;
        }

        public void SetSupplier(IWindowSupplier<TKey, TPopup> supplier)
        {
            this.supplier = supplier;
        }
    }
}