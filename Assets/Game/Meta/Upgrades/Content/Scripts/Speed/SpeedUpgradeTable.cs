using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    [Serializable]
    public sealed class SpeedUpgradeTable
    {
        public float SpeedStep
        {
            get { return speedStep; }
        }

        [Space]
        [InfoBox("Speed: Linear Function")]
        [SerializeField]
        private float startSpeed;

        [SerializeField]
        private float endSpeed;

        [ReadOnly]
        [SerializeField]
        private float speedStep;

        [Space]
        [ReadOnlyArray]
        [ListDrawerSettings(
            IsReadOnly = true,
            OnBeginListElementGUI = "DrawLabelForListElement"
        )]
        [SerializeField]
        private float[] table;

        public float GetSpeed(int level)
        {
            var index = level - 1;
            return table[index];
        }

        public void OnValidate(int maxLevel)
        {
            EvaluateTable(maxLevel);
        }

        private void EvaluateTable(int maxLevel)
        {
            table = new float[maxLevel];
            table[0] = startSpeed;
            table[maxLevel - 1] = endSpeed;

            var speedStep = (endSpeed - startSpeed) / (maxLevel - 1);
            this.speedStep = (float) Math.Round(speedStep, 2);

            for (var i = 1; i < maxLevel - 1; i++)
            {
                var speed = startSpeed + this.speedStep * i;
                table[i] = (float) Math.Round(speed, 2);
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