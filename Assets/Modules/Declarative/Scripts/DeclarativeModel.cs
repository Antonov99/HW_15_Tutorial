using System;
using System.Collections.Generic;
using UnityEngine;

namespace Declarative
{
    public abstract class DeclarativeModel : MonoBehaviour
    {
        private Dictionary<Type, object> sections;

        private MonoContext monoContext;

        public Action onAwake;
        public Action onEnable;
        public Action onStart;
        public Action<float> onUpdate;
        public Action<float> onFixedUpdate;
        public Action<float> onLateUpdate;
        public Action onDisable;
        public Action onDestroy;

        internal object GetSection(Type type)
        {
            return sections[type];
        }

        internal bool TryGetSection(Type type, out object section)
        {
            return sections.TryGetValue(type, out section);
        }

        private void Awake()
        {
            onAwake = null;
            onEnable = null;
            onStart = null;
            onUpdate = null;
            onFixedUpdate = null;
            onLateUpdate = null;
            onDisable = null;
            onDestroy = null;

            monoContext = new MonoContext(this);
            sections = SectionScanner.ScanSections(this);

            foreach (var section in sections.Values)
            {
                MonoContextInstaller.InstallElements(section, monoContext);
                SectionConstructor.ConstructSection(section, this);
            }

            monoContext.Awake();
            onAwake?.Invoke();
        }

        private void OnEnable()
        {
            monoContext.OnEnable();
            onEnable?.Invoke();
        }

        private void Start()
        {
            monoContext.Start();
            onStart?.Invoke();
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            monoContext.FixedUpdate(deltaTime);
            onFixedUpdate?.Invoke(deltaTime);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            monoContext.Update(deltaTime);
            onUpdate?.Invoke(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            monoContext.LateUpdate(deltaTime);
            onLateUpdate?.Invoke(deltaTime);
        }

        private void OnDisable()
        {
            monoContext.OnDisable();
            onDisable?.Invoke();
        }

        private void OnDestroy()
        {
            monoContext.OnDestroy();
            onDestroy?.Invoke();
        }

#if UNITY_EDITOR
        [ContextMenu("Construct")]
        private void Construct()
        {
            Awake();
            OnEnable();
            Debug.Log($"<color=#FF6235>: {name} successfully constructed!</color>");
        }

        [ContextMenu("Destruct")]
        private void Destruct()
        {
            if (monoContext != null)
            {
                monoContext.OnDisable();
                monoContext.OnDestroy();
            }

            Debug.Log($"<color=#FF6235>: {name} successfully destructed!</color>");
        }
#endif
    }
}