using System.Collections.Generic;
using System.Linq;

namespace GameSystem
{
    internal sealed class ElementContext
    {
        private readonly GameContext context;

        private readonly HashSet<IGameElement> gameElements;

        private readonly List<IGameElement> cache;

        private readonly List<IGameUpdateElement> updateListeners = new();

        private readonly List<IGameFixedUpdateElement> fixedUpdateListeners = new();

        private readonly List<IGameLateUpdateElement> lateUpdateListeners = new();

        internal ElementContext(GameContext context)
        {
            this.context = context;
            gameElements = new HashSet<IGameElement>();
            cache = new List<IGameElement>();
        }

        internal void AddElement(IGameElement element)
        {
            if (element == null)
            {
                return;
            }

            var addedElements = new HashSet<IGameElement>();
            AddRecursively(element, ref addedElements);

            foreach (var addedElement in addedElements)
            {
                TryActivateElement(addedElement);
                TryAddListener(addedElement);
            }
        }

        internal void RemoveElement(IGameElement element)
        {
            if (element != null)
            {
                RemoveRecursively(element);
            }
        }

        internal object[] GetAllElements()
        {
            return gameElements.ToArray<object>();
        }

        internal void ConstructGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGameConstructElement constructElement)
                {
                    constructElement.ConstructGame(context);
                }
            }
        }

        internal void InitGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGameInitElement initElement)
                {
                    initElement.InitGame();
                }
            }
        }

        internal void ReadyGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGameReadyElement initElement)
                {
                    initElement.ReadyGame();
                }
            }
        }

        internal void StartGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGameStartElement startElement)
                {
                    startElement.StartGame();
                }
            }
        }

        internal void FixedUpdate(float deltaTime)
        {
            for (int i = 0, count = fixedUpdateListeners.Count; i < count; i++)
            {
                var listener = fixedUpdateListeners[i];
                listener.OnFixedUpdate(deltaTime);
            }
        }

        internal void Update(float deltaTime)
        {
            for (int i = 0, count = updateListeners.Count; i < count; i++)
            {
                var listener = updateListeners[i];
                listener.OnUpdate(deltaTime);
            }
        }

        internal void LateUpdate(float deltaTime)
        {
            for (int i = 0, count = lateUpdateListeners.Count; i < count; i++)
            {
                var listener = lateUpdateListeners[i];
                listener.OnLateUpdate(deltaTime);
            }
        }

        internal void PauseGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGamePauseElement startElement)
                {
                    startElement.PauseGame();
                }
            }
        }

        internal void ResumeGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGameResumeElement startElement)
                {
                    startElement.ResumeGame();
                }
            }
        }

        internal void FinishGame()
        {
            cache.Clear();
            cache.AddRange(gameElements);

            for (int i = 0, count = cache.Count; i < count; i++)
            {
                var element = cache[i];
                if (element is IGameFinishElement finishElement)
                {
                    finishElement.FinishGame();
                }
            }
        }

        private void AddRecursively(IGameElement element, ref HashSet<IGameElement> addedElements)
        {
            if (gameElements.Add(element))
            {
                addedElements.Add(element);
            }

            if (element is IGameElementGroup elementGroup)
            {
                foreach (var child in elementGroup.GetElements())
                {
                    AddRecursively(child, ref addedElements);
                }
            }
        }

        private void RemoveRecursively(IGameElement element)
        {
            RemoveElementInternal(element);

            if (element is IGameElementGroup elementGroup)
            {
                foreach (var child in elementGroup.GetElements())
                {
                    RemoveRecursively(child);
                }
            }
        }

        private void RemoveElementInternal(IGameElement element)
        {
            gameElements.Remove(element);
            if (element is IGameDetachElement detachElement)
            {
                detachElement.DetachGame(context);
            }

            TryRemoveListener(element);
        }

        private void TryActivateElement(IGameElement element)
        {
            if (element is IGameAttachElement attachElement)
            {
                attachElement.AttachGame(context);
            }

            var gameState = context.CurrentState;
            if (gameState >= GameContext.State.FINISH)
            {
                return;
            }

            if (gameState < GameContext.State.CONSTRUCT)
            {
                return;
            }

            if (element is IGameConstructElement constructElement)
            {
                constructElement.ConstructGame(context);
            }

            if (gameState < GameContext.State.INIT)
            {
                return;
            }

            if (element is IGameInitElement initElement)
            {
                initElement.InitGame();
            }

            if (gameState < GameContext.State.READY)
            {
                return;
            }

            if (element is IGameReadyElement readyElement)
            {
                readyElement.ReadyGame();
            }

            if (gameState < GameContext.State.PLAY)
            {
                return;
            }

            if (element is IGameStartElement startElement)
            {
                startElement.StartGame();
            }

            if (gameState == GameContext.State.PAUSE && element is IGamePauseElement pauseElement)
            {
                pauseElement.PauseGame();
            }
        }

        private void TryAddListener(IGameElement listener)
        {
            if (listener is IGameUpdateElement updateElement)
            {
                updateListeners.Add(updateElement);
            }

            if (listener is IGameFixedUpdateElement fixedUpdateElement)
            {
                fixedUpdateListeners.Add(fixedUpdateElement);
            }

            if (listener is IGameLateUpdateElement lateUpdateElement)
            {
                lateUpdateListeners.Add(lateUpdateElement);
            }
        }

        private void TryRemoveListener(IGameElement listener)
        {
            if (listener is IGameUpdateElement updateElement)
            {
                updateListeners.Remove(updateElement);
            }

            if (listener is IGameFixedUpdateElement fixedUpdateElement)
            {
                fixedUpdateListeners.Remove(fixedUpdateElement);
            }

            if (listener is IGameLateUpdateElement lateUpdateElement)
            {
                lateUpdateListeners.Remove(lateUpdateElement);
            }
        }
    }
}