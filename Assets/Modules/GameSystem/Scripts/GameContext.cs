using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    [AddComponentMenu("GameSystem/Game Сontext")]
    [DisallowMultipleComponent]
    public class GameContext : MonoBehaviour
    {
        public enum State
        {
            OFF = 1,
            CONSTRUCT = 2,
            INIT = 3,
            READY = 4,
            PLAY = 5,
            PAUSE = 6,
            FINISH = 7
        }
        
        public event Action OnGameConstructed;
        
        public event Action OnGameInitialized;

        public event Action OnGameReady;

        public event Action OnGameStarted;

        public event Action OnGamePaused;

        public event Action OnGameResumed;

        public event Action OnGameFinished;

        public State CurrentState { get; protected set; }

        [SerializeField]
        private bool autoRun = true;
        
        [SerializeField, Space]
        private List<MonoBehaviour> gameServices = new();

        [SerializeField, Space]
        private List<MonoBehaviour> gameElements = new();

        [SerializeField, Space]
        private List<ConstructTask> constructTasks = new();

        private readonly ElementContext elementContext;

        private readonly ServiceContext serviceContext;

        public GameContext()
        {
            CurrentState = State.OFF;
            elementContext = new ElementContext(this);
            serviceContext = new ServiceContext();
        }

        private void Awake()
        {
            if (autoRun)
            {
                StartCoroutine(AutoRun());
            }
            else
            {
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            elementContext.FixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {
            elementContext.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            elementContext.LateUpdate(Time.deltaTime);
        }

        [ContextMenu("Construct Game")]
        public void ConstructGame()
        {
            if (CurrentState != State.OFF)
            {
                LogWarning($"Can't construct the game from the {CurrentState} state, " +
                           $"only from {nameof(State.OFF)}");
                return;
            }
            
            RegisterElements();
            RegisterServices();
            
            CurrentState = State.CONSTRUCT;
            elementContext.ConstructGame();

            foreach (var task in constructTasks)
            {
                task.Construct(this);
            }

            OnGameConstructed?.Invoke();
        }

        [ContextMenu("Init Game")]
        public void InitGame()
        {
            if (CurrentState != State.CONSTRUCT)
            {
                LogWarning($"Can't initialize the game from the {CurrentState} state, " +
                           $"only from {nameof(State.CONSTRUCT)}");
                return;
            }

            CurrentState = State.INIT;
            elementContext.InitGame();
            OnGameInitialized?.Invoke();
        }

        [ContextMenu("Ready Game")]
        public void ReadyGame()
        {
            if (CurrentState != State.INIT)
            {
                LogWarning($"Can't set ready the game from the {CurrentState} state, " +
                           $"only from {nameof(State.INIT)}");
                return;
            }

            CurrentState = State.READY;
            elementContext.ReadyGame();
            OnGameReady?.Invoke();
        }

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            if (CurrentState != State.READY)
            {
                LogWarning($"Can't start the game from the {CurrentState} state, " +
                           $"only from {nameof(State.READY)}");
                return;
            }

            CurrentState = State.PLAY;
            elementContext.StartGame();
            OnGameStarted?.Invoke();
            
            enabled = true;
        }

        [ContextMenu("Pause Game")]
        public void PauseGame()
        {
            if (CurrentState != State.PLAY)
            {
                LogWarning($"Can't pause the game from the {CurrentState} state, " +
                           $"only from {nameof(State.PLAY)}");
                return;
            }

            CurrentState = State.PAUSE;
            elementContext.PauseGame();
            OnGamePaused?.Invoke();
            
            enabled = false;
        }

        [ContextMenu("Resume Game")]
        public void ResumeGame()
        {
            if (CurrentState != State.PAUSE)
            {
                LogWarning($"Can't resume the game from the {CurrentState} state, " +
                           $"only from {nameof(State.PAUSE)}");
                return;
            }

            CurrentState = State.PLAY;
            elementContext.ResumeGame();
            OnGameResumed?.Invoke();
            
            enabled = true;
        }

        [ContextMenu("Finish Game")]
        public void FinishGame()
        {
            if (CurrentState is not (State.PLAY or State.PAUSE))
            {
                LogWarning($"Can't finish the game from the {CurrentState} state, " +
                           $"only from {nameof(State.PLAY)} or {nameof(State.PAUSE)}");
                return;
            }

            CurrentState = State.FINISH;
            elementContext.FinishGame();
            OnGameFinished?.Invoke();

            enabled = false;
        }

        private void RegisterElements()
        {
            for (int i = 0, count = gameElements.Count; i < count; i++)
            {
                var monoElement = gameElements[i];
                if (monoElement is IGameElement gameElement)
                {
                    RegisterElement(gameElement);
                }
            }
        }

        private void RegisterServices()
        {
            for (int i = 0, count = gameServices.Count; i < count; i++)
            {
                var monoService = gameServices[i];
                if (monoService != null)
                {
                    RegisterService(monoService);
                }
            }
        }

        private IEnumerator AutoRun()
        {
            yield return new WaitForEndOfFrame();
            ConstructGame();
            InitGame();
            ReadyGame();
            StartGame();
        }

#if UNITY_EDITOR
        public void Editor_AddElement(MonoBehaviour gameElement)
        {
            gameElements.Add(gameElement);
        }

        public void Editor_AddService(MonoBehaviour gameService)
        {
            gameServices.Add(gameService);
        }

        public void Editor_AddConstructTask(ConstructTask task)
        {
            constructTasks.Add(task);
        }

        private void OnValidate()
        {
            EditorValidator.ValidateServices(ref gameServices);
            EditorValidator.ValidateElements(ref gameElements);
        }
#endif

        public void RegisterElement(IGameElement element)
        {
            elementContext.AddElement(element);
        }

        public void UnregisterElement(IGameElement element)
        {
            elementContext.RemoveElement(element);
        }

        public object[] GetAllElements()
        {
            return elementContext.GetAllElements();
        }

        public void RegisterService(object service)
        {
            serviceContext.AddService(service);
        }

        public void UnregisterService(object service)
        {
            serviceContext.RemoveService(service);
        }

        public T GetService<T>()
        {
            return serviceContext.GetService<T>();
        }

        public object[] GetServices(Type type)
        {
            return serviceContext.GetServices(type);
        }

        public object[] GetAllServices()
        {
            return serviceContext.GetAllServices().ToArray();
        }

        public object GetService(Type type)
        {
            return serviceContext.GetService(type);
        }

        public bool TryGetService<T>(out T service)
        {
            return serviceContext.TryGetService(out service);
        }

        public bool TryGetService(Type type, out object service)
        {
            return serviceContext.TryGetService(type, out service);
        }

        public T[] GetServices<T>()
        {
            return serviceContext.GetServices<T>();
        }
        
        public abstract class ConstructTask : ScriptableObject
        {
            public abstract void Construct(GameContext gameContext);
        }

        private static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }
    }
}