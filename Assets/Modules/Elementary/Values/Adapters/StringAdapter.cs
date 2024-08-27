using System;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    [Serializable]
    public sealed class StringAdapter : IValue<string>
    {
        public string Current
        {
            get { return id; }
        }

        [Space]
        [SerializeField]
        public Mode mode = Mode.CUSTOM;

        [ShowIf("mode", Mode.SCRIPTABLE_OBJECT)]
        [OptionalField]
        [SerializeField]
        public ScriptableString scriptableString;

        [ShowIf("mode", Mode.CUSTOM)]
        [SerializeField]
        public string id;

        [ShowIf("mode", Mode.GAME_OBJECT)]
        [OptionalField]
        [SerializeField]
        public GameObject targetObject;

        public string GetValue()
        {
            if (mode == Mode.SCRIPTABLE_OBJECT)
            {
                return scriptableString.Current;
            }

            if (mode == Mode.CUSTOM)
            {
                return id;
            }

            if (mode == Mode.GAME_OBJECT)
            {
                return targetObject.name;
            }

            throw new Exception($"Mode {mode} is undefined!");
        }

        public enum Mode
        {
            SCRIPTABLE_OBJECT = 0,
            CUSTOM = 1,
            GAME_OBJECT = 2
        }
    }
}