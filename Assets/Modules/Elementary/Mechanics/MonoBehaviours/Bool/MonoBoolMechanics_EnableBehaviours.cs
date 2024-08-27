using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Mechanics/Bool Mechanics «Enable Behaviours»")]
    public sealed class MonoBoolMechanics_EnableBehaviours : MonoBoolMechanics
    {
        [Space]
        [SerializeField]
        private Behaviour[] behaviours;

        protected override void SetEnable(bool isEnable)
        {
            for (int i = 0, count = behaviours.Length; i < count; i++)
            {
                var behaviour = behaviours[i];
                behaviour.enabled = isEnable;
            }
        }
    }
}