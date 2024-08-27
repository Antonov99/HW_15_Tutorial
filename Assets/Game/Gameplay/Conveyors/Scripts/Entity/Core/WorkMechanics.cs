using Elementary;
using Game.GameEngine.Mechanics;
using Declarative;

namespace Game.Gameplay.Conveyors
{
    public sealed class WorkMechanics :
        IEnableListener,
        IDisableListener,
        IFixedUpdateListener
    {
        private IVariable<bool> isEnable;

        private IVariableLimited<int> loadStorage;

        private IVariableLimited<int> unloadStorage;

        private ITimer workTimer;

        public void Construct(
            IVariable<bool> isEnable,
            IVariableLimited<int> loadStorage,
            IVariableLimited<int> unloadStorage,
            ITimer workTimer
        )
        {
            this.isEnable = isEnable;
            this.loadStorage = loadStorage;
            this.unloadStorage = unloadStorage;
            this.workTimer = workTimer;
        }

        void IEnableListener.OnEnable()
        {
            workTimer.OnFinished += OnWorkFinished;
        }

        void IDisableListener.OnDisable()
        {
            workTimer.OnFinished -= OnWorkFinished;
        }

        void IFixedUpdateListener.FixedUpdate(float deltaTime)
        {
            if (!isEnable.Current)
            {
                return;
            }
            
            if (CanStartWork())
            {
                StartWork();
            }
        }

        private bool CanStartWork()
        {
            if (workTimer.IsPlaying)
            {
                return false;
            }

            if (loadStorage.Current == 0)
            {
                return false;
            }

            if (unloadStorage.IsLimit)
            {
                return false;
            }

            return true;
        }

        private void StartWork()
        {
            loadStorage.Current--;
            workTimer.ResetTime();
            workTimer.Play();
        }

        private void OnWorkFinished()
        {
            unloadStorage.Current++;
        }
    }
}