using System;
using System.Collections;
using Asyncoroutine;
using Game.Tutorial.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Tutorial.Gameplay
{
    [Serializable]
    public class InfoPanelShower
    {
        [Space]
        [SerializeField]
        private float showDelay = 1.0f;

        [SerializeField]
        private InfoPanel viewPrefab;

        protected InfoPanel view;
        
        public async void Show(Transform parent)
        {
            await new WaitForSeconds(showDelay);

            view = Object.Instantiate(viewPrefab, parent);
            OnShow();
        }

        public void Hide()
        {
            if (view != null)
            {
                OnHide();
                Object.Destroy(view.gameObject);
            }
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }
    }
}