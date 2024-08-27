using UnityEngine;

namespace Game.GameEngine
{
    public sealed class WorldPlaceObject : MonoBehaviour
    {
        public WorldPlaceType Type
        {
            get { return type; }
        }

        public Vector3 VisitPosition
        {
            get { return visitPoint.position; }
        }

        [SerializeField]
        private WorldPlaceType type;

        [SerializeField]
        private Transform visitPoint;
    }
}