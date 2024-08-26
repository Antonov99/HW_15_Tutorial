using Lessons.Game.Events.Effects;
using UnityEngine;

namespace Lessons.Game
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Lesson19/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeReference]
        public IEffect[] Effects;
    }
}