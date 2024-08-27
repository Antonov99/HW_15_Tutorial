using System.Collections.Generic;
using UnityEngine;

namespace Game.Meta
{
    public sealed class BoosterListView : MonoBehaviour
    {
        [SerializeField]
        private Transform container;

        [SerializeField]
        private BoosterView prefab;

        private readonly Dictionary<Booster, ViewHolder> boosters = new();

        public void AddBooster(Booster booster)
        {
            if (boosters.ContainsKey(booster))
            {
                return;
            }

            var view = Instantiate(prefab, container);
            var adapter = new BoosterViewAdapter(view, booster, coroutineDispatcher: this);
            var viewHolder = new ViewHolder(view, adapter);
            boosters.Add(booster, viewHolder);
            
            adapter.Show();
        }

        public void RemoveBooster(Booster booster)
        {
            if (!boosters.ContainsKey(booster))
            {
                return;
            }

            var viewHolder = boosters[booster];
            viewHolder.adapter.Hide();
            Destroy(viewHolder.view.gameObject);
            boosters.Remove(booster);
        }
        
        private struct ViewHolder
        {
            public readonly BoosterView view;
            public readonly BoosterViewAdapter adapter;

            public ViewHolder(BoosterView view, BoosterViewAdapter adapter)
            {
                this.view = view;
                this.adapter = adapter;
            }
        }
    }
}