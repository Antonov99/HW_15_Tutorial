using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Elementary
{
    /// <summary>
    ///     Receives animation events from animator
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Elementary/Animator/Animator Event Receiver")]
    public sealed class AnimatorEventReceiver : MonoBehaviour
    {
        public event Action OnEventReceived;

        public event Action<bool> OnBoolReceived;

        public event Action<int> OnIntReceived;

        public event Action<float> OnFloatReceived;

        public event Action<string> OnStringReceived;

        public event Action<object> OnObjectReceived;

        [Button, GUIColor(0, 1, 0)]
        public void ReceiveEvent()
        {
            OnEventReceived?.Invoke();
        }

        [PropertySpace, Button, GUIColor(0, 1, 0)]
        public void ReceiveString(string message)
        {
            OnStringReceived?.Invoke(message);
        }

        [Button, GUIColor(0, 1, 0)]
        public void ReceiveBool(bool message)
        {
            OnBoolReceived?.Invoke(message);
        }

        [Button, GUIColor(0, 1, 0)]
        public void ReceiveInt(int message)
        {
            OnIntReceived?.Invoke(message);
        }

        [Button, GUIColor(0, 1, 0)]
        public void ReceiveFloat(float message)
        {
            OnFloatReceived?.Invoke(message);
        }

        [Button, GUIColor(0, 1, 0)]
        public void ReceiveObject(object obj)
        {
            OnObjectReceived?.Invoke(obj);
        }
    }
}