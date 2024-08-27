using System.Collections.Generic;
using UnityEngine;

namespace Declarative
{
    internal sealed class MonoContext
    {
        private readonly MonoBehaviour root;
        
        private readonly List<IAwakeListener> awakeComponents = new();

        private readonly List<IEnableListener> enableComponents = new();

        private readonly List<IStartListener> startComponents = new();

        private readonly List<IFixedUpdateListener> fixedUpdateComponents = new();

        private readonly List<IUpdateListener> updateComponents = new();

        private readonly List<ILateUpdateListener> lateUpdateComponents = new();

        private readonly List<IDisableListener> disableComponents = new();

        private readonly List<IDestroyListener> destroyComponents = new();

        public MonoContext(MonoBehaviour root)
        {
            this.root = root;
        }

        internal void AddListener(IMonoElement element)
        {
            if (element is IMonoInjective injectiveComponent)
            {
                injectiveComponent.Context = root;
            }
            
            if (element is IAwakeListener awakeComponent)
            {
                awakeComponents.Add(awakeComponent);
            }

            if (element is IEnableListener enableComponent)
            {
                enableComponents.Add(enableComponent);
            }

            if (element is IStartListener startComponent)
            {
                startComponents.Add(startComponent);
            }

            if (element is IFixedUpdateListener fixedUpdateComponent)
            {
                fixedUpdateComponents.Add(fixedUpdateComponent);
            }

            if (element is IUpdateListener updateComponent)
            {
                updateComponents.Add(updateComponent);
            }

            if (element is ILateUpdateListener lateUpdateComponent)
            {
                lateUpdateComponents.Add(lateUpdateComponent);
            }

            if (element is IDisableListener disableComponent)
            {
                disableComponents.Add(disableComponent);
            }
        }

        public void Awake()
        {
            for (int i = 0, count = awakeComponents.Count; i < count; i++)
            {
                var listener = awakeComponents[i];
                listener.Awake();
            }
        }

        public void OnEnable()
        {
            for (int i = 0, count = enableComponents.Count; i < count; i++)
            {
                var listener = enableComponents[i];
                listener.OnEnable();
            }
        }

        public void Start()
        {
            for (int i = 0, count = startComponents.Count; i < count; i++)
            {
                var listener = startComponents[i];
                listener.Start();
            }
        }

        public void FixedUpdate(float deltaTime)
        {
            for (int i = 0, count = fixedUpdateComponents.Count; i < count; i++)
            {
                var listener = fixedUpdateComponents[i];
                listener.FixedUpdate(deltaTime);
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0, count = updateComponents.Count; i < count; i++)
            {
                var listener = updateComponents[i];
                listener.Update(deltaTime);
            }
        }

        public void LateUpdate(float deltaTime)
        {
            for (int i = 0, count = lateUpdateComponents.Count; i < count; i++)
            {
                var listener = lateUpdateComponents[i];
                listener.LateUpdate(deltaTime);
            }
        }

        public void OnDisable()
        {
            for (int i = 0, count = disableComponents.Count; i < count; i++)
            {
                var listener = disableComponents[i];
                listener.OnDisable();
            }
        }

        public void OnDestroy()
        {
            for (int i = 0, count = destroyComponents.Count; i < count; i++)
            {
                var listener = destroyComponents[i];
                listener.OnDestroy();
            }
        }
    }
}