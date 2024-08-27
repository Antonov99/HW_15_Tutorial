using System.Collections.Generic;
using GameSystem;
using UnityEngine;

namespace GameSystem.Extensions
{
    [AddComponentMenu("GameSystem/Game Element Group «Components In Children»")]
    public sealed class GameElementGroup_ComponentsInChildren : MonoBehaviour, IGameElementGroup
    {
        [SerializeField]
        private bool includeInactive = true;
        
        public IEnumerable<IGameElement> GetElements()
        {
            var results = new List<IGameElement>(capacity: 0);
            if (gameObject.activeSelf)
            {
                GetComponentsInChildren<IGameElement>(includeInactive, results);
                results.Remove(this);
            }

            return results;
        }
    }
}