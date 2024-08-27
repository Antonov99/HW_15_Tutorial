using System.Collections.Generic;
using GameSystem;

namespace Game.App
{
    public sealed class GameFacade
    {
        private readonly List<object> registeredServices;
        
        private readonly List<IGameElement> registeredElements;
        
        private GameContext context;

        public GameFacade()
        {
            registeredServices = new List<object>();
            registeredElements = new List<IGameElement>();
        }
        
        public void SetupGame(GameContext context)
        {
            for (int i = 0, count = registeredServices.Count; i < count; i++)
            {
                var service = registeredServices[i];
                context.RegisterService(service);
            }
        
            for (int i = 0, count = registeredElements.Count; i < count; i++)
            {
                var element = registeredElements[i];
                context.RegisterElement(element);
            }

            this.context = context;
        }

        public void ConstructGame()
        {
            context.ConstructGame();
        }

        public void InitGame()
        {
            context.InitGame();
        }

        public void ReadyGame()
        {
            context.ReadyGame();
        }

        public void StartGame()
        {
            context.StartGame();
        }

        public T GetService<T>()
        {
            return context.GetService<T>();
        }

        public T[] GetServices<T>()
        {
            return context.GetServices<T>();
        }

        public bool TryGetService<T>(out T result)
        {
            return context.TryGetService(out result);
        }

        public void RegisterService(object service)
        {
            registeredServices.Add(service);
            if (context != null)
            {
                context.RegisterService(service);
            }
        }

        public void UnregisterService(object service)
        {
            registeredServices.Remove(service);
            if (context != null)
            {
                context.UnregisterService(service);
            }
        }

        public void RegisterElement(IGameElement element)
        {
            registeredElements.Add(element);
            if (context != null)
            {
                context.RegisterElement(element);
            }
        }

        public void UnregisterElement(IGameElement element)
        {
            registeredElements.Remove(element);
            if (context != null)
            {
                context.UnregisterElement(element);
            }
        }
    }
}