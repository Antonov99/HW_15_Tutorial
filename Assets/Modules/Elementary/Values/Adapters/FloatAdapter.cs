using System;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class FloatAdapter : IValue<float>
    {
        public float Current
        {
            get { return GetValue(); }
        }

        [SerializeField]
        public Mode mode;

        [ShowIf("mode", Mode.BASE)]
        [SerializeField]
        public float baseValue;

        [ShowIf("mode", Mode.MONO_BEHAVIOUR)]
        [OptionalField]
        [SerializeField]
        public MonoFloatVariable monoValue;

        [ShowIf("mode", Mode.SCRIPTABLE_OBJECT)]
        [OptionalField]
        [SerializeField]
        public ScriptableFloat scriptableValue;

        public float GetValue()
        {
            return mode switch
            {
                Mode.BASE => baseValue,
                Mode.MONO_BEHAVIOUR => monoValue.Current,
                Mode.SCRIPTABLE_OBJECT => scriptableValue.Current,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public enum Mode
        {
            BASE = 0,
            MONO_BEHAVIOUR = 1,
            SCRIPTABLE_OBJECT = 2
        }
    }
}