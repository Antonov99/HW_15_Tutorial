using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public interface IGameServiceGroup
    {
        IEnumerable<object> GetServices();
    }
    
    [AddComponentMenu("GameSystem/Game Service Group")]
    [DisallowMultipleComponent]
    public sealed class GameServiceGroup : MonoBehaviour, IGameServiceGroup
    {
        [SerializeField]
        private List<MonoBehaviour> gameServices = new();

        public IEnumerable<object> GetServices()
        {
            for (int i = 0, count = gameServices.Count; i < count; i++)
            {
                yield return gameServices[i];
            }
        }

#if UNITY_EDITOR
        public void Editor_AddService(MonoBehaviour service)
        {
            gameServices.Add(service);
        }
        
        private void OnValidate()
        {
            EditorValidator.ValidateServices(ref gameServices);
        }
#endif
    }
}