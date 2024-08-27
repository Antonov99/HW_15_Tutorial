using UnityEngine;

namespace Game.GameEngine
{
    public sealed class WorldPlaceTrigger : MonoBehaviour
    {
        public WorldPlaceType PlaceType
        {
            get { return worldPlace.Type; }
        }

        [SerializeField]
        private WorldPlaceObject worldPlace;
    }
}