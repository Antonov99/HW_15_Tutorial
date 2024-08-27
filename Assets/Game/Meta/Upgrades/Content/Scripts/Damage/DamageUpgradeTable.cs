using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    [Serializable]
    public sealed class DamageUpgradeTable
    {
        public int DamageStep
        {
            get { return damageStep; }
        }

        [InfoBox("Damage: Linear Function")]
        [SerializeField]
        private int startDamage = 1;

        [SerializeField]
        private int damageStep = 1;

        [Space]
        [ReadOnlyArray]
        [ListDrawerSettings(
            IsReadOnly = true,
            OnBeginListElementGUI = "DrawLabelForListElement"
        )]
        [SerializeField]
        private int[] levels;

        public int GetDamage(int level)
        {
            var index = level - 1;
            return levels[index];
        }

        public void OnValidate(int maxLevel)
        {
            levels = new int[maxLevel];

            var currentDamage = startDamage;
            for (var i = 0; i < maxLevel; i++)
            {
                levels[i] = currentDamage;
                currentDamage += damageStep;
            }
        }

#if UNITY_EDITOR
        private void DrawLabelForListElement(int index)
        {
            GUILayout.Space(8);
            GUILayout.Label($"Level {index + 1}");
        }
#endif
    }
}