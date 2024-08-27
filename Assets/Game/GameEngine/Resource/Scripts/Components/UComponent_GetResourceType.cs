using UnityEngine;

namespace Game.GameEngine.GameResources
{
    [AddComponentMenu("GameEngine/GameResources/Component «Get Resource Type»")]
    public sealed class UComponent_GetResourceType : MonoBehaviour, IComponent_GetResourceType
    {
        public ResourceType Type
        {
            get { return type; }
        }

        [SerializeField]
        private ResourceType type;
    }
}