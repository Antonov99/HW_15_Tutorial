using Elementary;
using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [AddComponentMenu("GameEngine/GameResources/Component «Get Resource Count»")]
    public sealed class UComponent_GetResourceCount : MonoBehaviour, IComponent_GetResourceCount
    {
        public int Count
        {
            get { return adapter.Current; }
        }
        
        [SerializeField]
        private IntAdapter adapter;
    }
}