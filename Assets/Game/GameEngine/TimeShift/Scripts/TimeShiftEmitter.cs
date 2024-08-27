using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class TimeShiftEmitter : MonoBehaviour
    {
        public event TimeShiftDelegate OnTimeShifted;
        
        [SerializeField]
        private bool isEnable = true;

        private readonly List<ITimeShiftHandler> handlers = new();

        [Button]
        [ShowIf("isEnable")]
        [GUIColor(0, 1, 0)]
        public void EmitEvent(TimeShiftReason reason, float shiftSeconds)
        {
            if (!isEnable)
            {
                return;
            }

            for (int i = 0, count = handlers.Count; i < count; i++)
            {
                var listener = handlers[i];
                listener.OnTimeShifted(reason, shiftSeconds);
            }

            OnTimeShifted?.Invoke(reason, shiftSeconds);
        }

        public void AddHandler(ITimeShiftHandler handler)
        {
            handlers.Add(handler);
        }

        public void RemoveHandler(ITimeShiftHandler handler)
        {
            handlers.Remove(handler);
        }
    }
}