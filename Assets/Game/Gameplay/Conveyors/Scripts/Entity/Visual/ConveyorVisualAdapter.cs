using Elementary;
using Declarative;
using UnityEngine;

namespace Game.Gameplay.Conveyors
{
    public sealed class ConveyorVisualAdapter :
        IEnableListener,
        IDisableListener
    {
        private ITimer workTimer;

        private ConveyorVisual conveyor;

        public void Construct(ITimer workTimer, ConveyorVisual conveyor)
        {
            this.workTimer = workTimer;
            this.conveyor = conveyor;
        }

        void IEnableListener.OnEnable()
        {
            workTimer.OnStarted += OnStartWork;
            workTimer.OnFinished += OnFinishWork;
        }

        void IDisableListener.OnDisable()
        {
            workTimer.OnStarted -= OnStartWork;
            workTimer.OnFinished -= OnFinishWork;
        }

        private void OnStartWork()
        {
            conveyor.Play();
        }

        private void OnFinishWork()
        {
            conveyor.Stop();
        }
    }
}