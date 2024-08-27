using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.App
{
    public sealed class ApplicationManager : MonoBehaviour
    {
        public event Action<float> OnUpdate;

        public event Action OnPaused;

        public event Action OnResumed;

        public event Action OnQuit;

        private readonly List<IAppUpdateListener> updateListeners = new();

        private readonly List<IAppPauseListener> pauseListeners = new();

        private readonly List<IAppResumeListener> resumeListeners = new();

        private readonly List<IAppQuitListener> quitListeners = new();

        private void Update()
        {
            InvokeUpdate();
        }

#if UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                InvokeResume();
            }
            else
            {
                InvokePause();
            }
        }

#else
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                this.InvokePause();
            }
            else
            {
                this.InvokeResume();
            }
        }
#endif

        private void OnApplicationQuit()
        {
            InvokeQuit();
        }
        
        public void AddListener(object listener)
        {
            if (listener is IAppUpdateListener updateListener)
            {
                updateListeners.Add(updateListener);
            }

            if (listener is IAppPauseListener pauseListener)
            {
                pauseListeners.Add(pauseListener);
            }
            
            if (listener is IAppResumeListener resumeListener)
            {
                resumeListeners.Add(resumeListener);
            }

            if (listener is IAppQuitListener quitListener)
            {
                quitListeners.Add(quitListener);
            }
        }
        
        public void RemoveListener(object listener)
        {
            if (listener is IAppUpdateListener updateListener)
            {
                updateListeners.Remove(updateListener);
            }

            if (listener is IAppPauseListener pauseListener)
            {
                pauseListeners.Remove(pauseListener);
            }
            
            if (listener is IAppResumeListener resumeListener)
            {
                resumeListeners.Remove(resumeListener);
            }

            if (listener is IAppQuitListener quitListener)
            {
                quitListeners.Remove(quitListener);
            }
        }

        private void InvokeUpdate()
        {
            var deltaTime = Time.deltaTime;
            for (int i = 0, count = updateListeners.Count; i < count; i++)
            {
                var listener = updateListeners[i];
                listener.OnUpdate(deltaTime);
            }

            OnUpdate?.Invoke(deltaTime);
        }

        private void InvokePause()
        {
            for (int i = 0, count = pauseListeners.Count; i < count; i++)
            {
                var listener = pauseListeners[i];
                listener.OnPaused();
            }

            OnPaused?.Invoke();
        }

        private void InvokeResume()
        {
            for (int i = 0, count = resumeListeners.Count; i < count; i++)
            {
                var listener = resumeListeners[i];
                listener.OnResumed();
            }

            OnResumed?.Invoke();
        }

        private void InvokeQuit()
        {
            for (int i = 0, count = quitListeners.Count; i < count; i++)
            {
                var listener = quitListeners[i];
                listener.OnQuit();
            }

            OnQuit?.Invoke();
        }
    }
}