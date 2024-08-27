using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public interface IGameElementGroup : IGameElement
    {
        IEnumerable<IGameElement> GetElements();
    }
    
    [AddComponentMenu("GameSystem/Game Element Group")]
    [DisallowMultipleComponent]
    public sealed class GameElementGroup : MonoBehaviour, IGameElementGroup
    {
        [SerializeField]
        private List<MonoBehaviour> gameElements = new();

        public IEnumerable<IGameElement> GetElements()
        {
            for (int i = 0, count = gameElements.Count; i < count; i++)
            {
                yield return (IGameElement) gameElements[i];
            }
        }

#if UNITY_EDITOR
        public void Editor_AddElement(IGameElement element)
        {
            gameElements.Add((MonoBehaviour) element);
        }

        private void OnValidate()
        {
            EditorValidator.ValidateElements(ref gameElements);
        }
#endif
    }
}