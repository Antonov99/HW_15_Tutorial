using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Conveyors
{
    [AddComponentMenu("Gameplay/Conveyors/Conveyor Zone Visual")]
    public sealed class ZoneVisual : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> items;

        private int currentAmount;

        public void SetupItems(int currentAmount)
        {
            currentAmount = Mathf.Clamp(currentAmount, 0, items.Count);
            this.currentAmount = currentAmount;

            for (var i = 0; i < currentAmount; i++)
            {
                var item = items[i];
                item.SetActive(true);
            }

            var count = items.Count;
            for (var i = currentAmount; i < count; i++)
            {
                var item = items[i];
                item.SetActive(false);
            }
        }

        public void IncrementItems(int range)
        {
            var previousAmount = currentAmount;
            var newAmount = Mathf.Min(currentAmount + range, items.Count);
            currentAmount = newAmount;

            for (var i = previousAmount; i < newAmount; i++)
            {
                var item = items[i];
                item.SetActive(true);
            }
        }

        public void DecrementItems(int range)
        {
            var previousAmount = currentAmount;
            var newAmount = Mathf.Max(currentAmount - range, 0);
            currentAmount = newAmount;

            for (var i = previousAmount - 1; i >= newAmount; i--)
            {
                var item = items[i];
                item.SetActive(false);
            }
        }

#if UNITY_EDITOR
        [Button("Setup Items")]
        private void Editor_SetupItems()
        {
            items = new List<GameObject>();
            foreach (Transform child in transform)
            {
                items.Add(child.gameObject);
            }
        }
#endif
    }
}