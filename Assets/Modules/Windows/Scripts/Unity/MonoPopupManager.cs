using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Windows
{
    public abstract class MonoPopupManager<T> : MonoBehaviour, IPopupManager<T>
    {
        public event Action<T> OnPopupShown;

        public event Action<T> OnPopupHidden;
        
        protected abstract IWindowSupplier<T, MonoWindow> Supplier { get; }

        private IPopupManager<T> manager;

        protected virtual void Awake()
        {
            manager = new PopupManager<T, MonoWindow>(Supplier);
        }

        protected virtual void OnEnable()
        {
            manager.OnPopupShown += OnShowPopup;
            manager.OnPopupHidden += OnHidePopup;
        }

        protected virtual void OnDisable()
        {
            manager.OnPopupShown -= OnShowPopup;
            manager.OnPopupHidden -= OnHidePopup;
        }

        [Button]
        public void ShowPopup(T key, object args = null)
        {
            manager.ShowPopup(key, args);
        }

        [Button]
        public void HidePopup(T key)
        {
            manager.HidePopup(key);
        }

        [Button]
        public void HideAllPopups()
        {
            manager.HideAllPopups();
        }

        public bool IsPopupActive(T popupName)
        {
            return manager.IsPopupActive(popupName);
        }

        private void OnShowPopup(T popupName)
        {
            OnPopupShown?.Invoke(popupName);
        }

        private void OnHidePopup(T popupName)
        {
            OnPopupHidden?.Invoke(popupName);
        }
    }
}