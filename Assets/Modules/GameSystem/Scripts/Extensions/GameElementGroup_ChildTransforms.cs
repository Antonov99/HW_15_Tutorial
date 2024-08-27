using System.Collections.Generic;
using GameSystem;
using UnityEngine;

namespace GameSystem.Extensions
{
    [AddComponentMenu("GameSystem/Game Element Group «Child Transforms»")]
    public sealed class GameElementGroup_ChildTransforms : MonoBehaviour, IGameElementGroup
    {
        [SerializeField]
        private bool includeInactive;
        
        public IEnumerable<IGameElement> GetElements()
        {
            if (!gameObject.activeSelf)
            {
                yield break;
            }

            if (includeInactive)
            {
                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent(out IGameElement gameElement))
                    {
                        yield return gameElement;
                    }
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject.activeSelf && 
                        child.TryGetComponent(out IGameElement gameElement))
                    {
                        yield return gameElement;
                    }
                }
            }
        }
    }
}