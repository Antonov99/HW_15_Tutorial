using Elementary;
using Sirenix.OdinInspector;

namespace Game.GameEngine.Mechanics
{
    public sealed class HarvestResourceState_TimeProgress : StateFixedUpdate
    {
        private IOperator<HarvestResourceOperation> harvestOperator;

        private IValue<float> duration;

        [ReadOnly, ShowInInspector]
        private float currentTime;

        public void ConstructOperator(IOperator<HarvestResourceOperation> harvestOperator)
        {
            this.harvestOperator = harvestOperator;
        }

        public void ConstructDuration(IValue<float> duration)
        {
            this.duration = duration;
        }

        protected override void OnEnter()
        {
            currentTime = harvestOperator.Current.progress * duration.Current;
        }

        protected override void FixedUpdate(float deltaTime)
        {
            if (harvestOperator.IsActive)
            {
                if (currentTime < duration.Current)
                {
                    UpdateProgress(deltaTime);
                }
                else
                {
                    Complete();
                }    
            }
        }

        private void UpdateProgress(float deltaTime)
        {
            currentTime += deltaTime;
            var progress = currentTime / duration.Current;
            harvestOperator.Current.progress = progress;
        }

        private void Complete()
        {
            var operation = harvestOperator.Current;
            operation.isCompleted = true;
            operation.progress = 1.0f;
            harvestOperator.Stop();
        }
    }
}