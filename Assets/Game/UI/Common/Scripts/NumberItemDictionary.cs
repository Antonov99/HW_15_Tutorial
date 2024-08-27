using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public abstract class NumberItemDictionary<K> : MonoBehaviour
    {
        [SerializeField]
        private Transform itemsContainer;

        [SerializeField]
        private NumberItem itemPrefab;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("hideItemIfZero")]
        private bool controlItemVisibility = true;

        [ShowIf("controlItemVisibility")]
        [SerializeField]
        private bool changeItemOrder = true;

        private readonly Dictionary<K, NumberItem> items;

        public NumberItemDictionary()
        {
            items = new Dictionary<K, NumberItem>();
        }

        public void AddItem(K key, int amount)
        {
            var view = Instantiate(itemPrefab, itemsContainer);
            view.name = key.ToString();
            view.SetIcon(FindIcon(key));
            view.SetupNumber(amount);
            items.Add(key, view);
            UpdateItemVisibility(view);
        }

        public void RemoveItem(K key)
        {
            var view = items[key];
            items.Remove(key);
            Destroy(view.gameObject);
        }

        public void ResetAllItems()
        {
            foreach (var view in items.Values)
            {
                view.SetupNumber(0);
                UpdateItemVisibility(view);
            }
        }

        public void SetupItem(K key, int amount)
        {
            var view = items[key];
            view.SetupNumber(amount);
            UpdateItemVisibility(view);
        }

        public void UpdateItem(K key, int amount)
        {
            var view = items[key];
            view.UpdateNumber(amount);
            UpdateItemVisibility(view);
        }

        public void IncrementItem(K key, int range)
        {
            var view = items[key];
            view.IncrementNumber(range);
            UpdateItemVisibility(view);
        }

        public void DecrementItem(K key, int range)
        {
            var view = items[key];
            view.DecrementNumber(range);
            UpdateItemVisibility(view);
        }

        public bool IsItemShown(K key)
        {
            var view = items[key];
            return view.gameObject.activeInHierarchy;
        }

        public void ShowItem(K key)
        {
            var view = items[key];
            view.gameObject.SetActive(true);
        }

        public void HideItem(K key)
        {
            var view = items[key];
            view.gameObject.SetActive(false);
        }
        
        public Vector3 GetIconCenter(K key)
        {
            var view = items[key];
            return view.GetIconCenter();
        }

        public int GetCurrentValue(K key)
        {
            var view = items[key];
            return view.CurrentValue;
        }

        protected abstract Sprite FindIcon(K key);

        private void UpdateItemVisibility(NumberItem view)
        {
            if (!controlItemVisibility)
            {
                return;
            }

            var isVisible = view.CurrentValue > 0;
            var viewObject = view.gameObject;
            if (viewObject.activeSelf == isVisible)
            {
                return;    
            }
                
            viewObject.SetActive(isVisible);

            if (changeItemOrder)
            {
                view.transform.SetAsLastSibling();
            }
        }
    }
}