using System;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine
{
    [Serializable]
    public sealed class ObjectTypeAdapter
    {
        public ObjectType Value
        {
            get { return GetValue(); }
        }

        [SerializeField]
        private Mode mode = Mode.BASE;

        [Space]
        [ShowIf("mode", Mode.BASE)]
        [SerializeField]
        private ObjectType baseObjectType;

        [Space]
        [ShowIf("mode", Mode.SCRIPTABLE_OBJECT)]
        [OptionalField]
        [SerializeField]
        private ScriptableObjectType scriptableObjectType;

        private ObjectType GetValue()
        {
            if (mode == Mode.BASE)
            {
                return baseObjectType;
            }

            if (mode == Mode.SCRIPTABLE_OBJECT)
            {
                return scriptableObjectType.ObjectType;
            }

            throw new Exception($"Mode {mode} is undefined!");
        }

        private enum Mode
        {
            BASE = 0,
            SCRIPTABLE_OBJECT = 1
        }
    }
}