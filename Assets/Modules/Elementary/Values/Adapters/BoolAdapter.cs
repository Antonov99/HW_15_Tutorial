using System;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class BoolAdapter : IValue<bool>
    {
        public bool Current
        {
            get { return GetValue(); }
        }

        [SerializeField]
        public Mode mode;

        [ShowIf("mode", Mode.BASE)]
        [SerializeField]
        public bool baseValue;

        [ShowIf("mode", Mode.MONO_BEHAVIOUR)]
        [OptionalField]
        [SerializeField]
        public MonoBoolVariable monoValue;

        [ShowIf("mode", Mode.SCRIPTABLE_OBJECT)]
        [OptionalField]
        [SerializeField]
        public ScriptableBool scriptableValue;

        public bool GetValue()
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