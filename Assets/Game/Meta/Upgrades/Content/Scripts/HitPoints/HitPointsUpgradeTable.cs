using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    [Serializable]
    public sealed class HitPointsUpgradeTable
    {
        public int HitPointsStep
        {
            get { return hitPointsStep; }
        }

        [InfoBox("Hit Points: Linear Function")]
        [SerializeField]
        private int startHitPoints = 1;

        [SerializeField]
        private int hitPointsStep = 1;

        [Space]
        [ReadOnlyArray]
        [ListDrawerSettings(
            IsReadOnly = true,
            OnBeginListElementGUI = "DrawLabelForListElement"
        )]
        [SerializeField]
        private int[] levels;

        public int GetHitPoints(int level)
        {
            var index = level - 1;
            return levels[index];
        }

        public void OnValidate(int maxLevel)
        {
            var levels = new int[maxLevel];

            var currentHitPoints = startHitPoints;
            for (var i = 0; i < maxLevel; i++)
            {
                levels[i] = currentHitPoints;
                currentHitPoints += hitPointsStep;
            }

            this.levels = levels;
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