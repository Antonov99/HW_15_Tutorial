using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Equip/Component «Get Equip Type»")]
    public sealed class UComponent_EquipType : MonoBehaviour, IComponent_GetEqupType
    {
        public EquipType Type
        {
            get { return type; }
        }

        [SerializeField]
        private EquipType type;
    }
}