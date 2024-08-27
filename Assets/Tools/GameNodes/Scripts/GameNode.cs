using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace GameNodes
{
    public class GameNode : MonoBehaviour
    {
        private GameNode parent;

        [SerializeField]
        private List<GameNode> children;

        private readonly List<object> services = new();

        private readonly List<IGameUpdater> updaters = new();

        private readonly List<IGameFixedUpdater> fixedUpdaters = new();

        private readonly List<IGameLateUpdater> lateUpdaters = new();

        private bool installed;

        #region Unity

        private void Update()
        {
            if (!installed)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            for (int i = 0, count = updaters.Count; i < count; i++)
            {
                var listener = updaters[i];
                listener.Update(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (!installed)
            {
                return;
            }

            var deltaTime = Time.fixedDeltaTime;
            for (int i = 0, count = fixedUpdaters.Count; i < count; i++)
            {
                var listener = fixedUpdaters[i];
                listener.FixedUpdate(deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (!installed)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            for (int i = 0, count = lateUpdaters.Count; i < count; i++)
            {
                var listener = lateUpdaters[i];
                listener.LateUpdate(deltaTime);
            }
        }

        #endregion

        #region Install

        [ContextMenu("Install")]
        public Task InstallAsync()
        {
            return Task.Run(Install);
        }

        [ContextMenu("Install")]
        public void Install()
        {
            if (installed)
            {
                Debug.LogWarning($"Game Node {name} is already installed", this);
                return;
            }

            foreach (var service in LoadServices())
            {
                SetupService(service);
            }

            installed = true;

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                node.parent = this;
                node.Install();
            }
        }

        protected virtual IEnumerable<object> LoadServices()
        {
            yield break;
        }

        private void SetupService(object service)
        {
            services.Add(service);

            if (service is IGameUpdater updater)
            {
                updaters.Add(updater);
            }

            if (service is IGameFixedUpdater fixedUpdater)
            {
                fixedUpdaters.Add(fixedUpdater);
            }

            if (service is IGameLateUpdater lateUpdater)
            {
                lateUpdaters.Add(lateUpdater);
            }
        }

        #endregion

        #region Send

        public Task SendAsync<T>() where T : GameEvent
        {
            return Task.Run(Send<T>);
        }

        public void Send<T>() where T : GameEvent
        {
            if (!installed)
            {
                Debug.LogWarning($"Game Node {name} is not installed yet", this);
                return;
            }

            InvokeServices<T>();

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                node.Send<T>();
            }
        }

        private void InvokeServices<T>() where T : GameEvent
        {
            for (int i = 0, count = services.Count; i < count; i++)
            {
                var service = services[i];
                InvokeService<T>(service);
            }
        }

        private void InvokeService<T>(object service) where T : GameEvent
        {
            var type = service.GetType();
            while (type != null && type != typeof(object) && type != typeof(MonoBehaviour))
            {
                var methods = type.GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly
                );

                for (int i = 0, count = methods.Length; i < count; i++)
                {
                    var method = methods[i];
                    if (method.GetCustomAttribute<T>() != null)
                    {
                        InvokeServiceMethod(service, method);
                    }
                }

                type = type.BaseType;
            }
        }
        
        private void InvokeServiceMethod(object service, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var count = parameters.Length;

            var args = new object[count];
            for (var i = 0; i < count; i++)
            {
                var parameter = parameters[i];
                var parameterType = parameter.ParameterType;
                args[i] = Service(parameterType);
            }

            method.Invoke(service, args);
        }

        #endregion

        #region Services
        
        public object Service(Type type)
        {
            var node = this;
            while (node != null)
            {
                if (node.FindService(type, out var service))
                {
                    return service;
                }

                node = node.parent;
            }

            throw new Exception($"Can't find service {type.Name}!");
        }

        public T Service<T>()
        {
            var node = this;
            while (node != null)
            {
                if (node.FindService<T>(out var service))
                {
                    return service;
                }

                node = node.parent;
            }

            throw new Exception($"Can't find service {typeof(T).Name}!");
        }
        
        public IEnumerable<T> Services<T>()
        {
            var node = this;
            while (node != null)
            {
                if (node.FindService<T>(out var service))
                {
                    yield return service;
                }

                node = node.parent;
            }
        }

        private bool FindService<T>(out T service)
        {
            for (int i = 0, count = services.Count; i < count; i++)
            {
                var current = services[i];
                if (current is T tService)
                {
                    service = tService;
                    return true;
                }
            }

            service = default;
            return false;
        }

        private bool FindService(Type targetType, out object service)
        {
            for (int i = 0, count = services.Count; i < count; i++)
            {
                service = services[i];
                var serviceType = service.GetType();
                if (targetType.IsAssignableFrom(serviceType))
                {
                    return true;
                }
            }

            service = default;
            return false;
        }

        #endregion

        #region Node

        public T Node<T>(Func<T, bool> predicate = null) where T : GameNode
        {
            if (predicate == null)
            {
                predicate = _ => true;
            }

            for (int i = 0, count = children.Count; i < count; i++)
            {
                var node = children[i];
                if (node is not T tNode)
                {
                    continue;
                }

                if (predicate.Invoke(tNode))
                {
                    return tNode;
                }
            }

            throw new Exception($"Node of type {typeof(T).Name} is not found!");
        }

        public void AddNode(GameNode node)
        {
            children.Add(node);
            node.parent = this;
            node.Install();
        }

        public void RemoveNode(GameNode node)
        {
            if (children.Remove(node))
            {
                node.parent = null;
            }
        }

        #endregion
    }
}