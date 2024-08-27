using System.Collections;
using Elementary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Harvest Resource State «Time Progress»")]
    public sealed class UHarvestResourceState_TimeProgress : MonoStateCoroutine
    {
        [SerializeField]
        private UHarvestResourceOperator engine;
        
        [Space]
        [SerializeField]
        private FloatAdapter workTime;
        
        [ReadOnly]
        [ShowInInspector]
        private float currentTime;

        public override void Enter()
        {
            currentTime = engine.Current.progress * workTime.Current;
            base.Enter();
        }

        protected override IEnumerator Do()
        {
            while (currentTime < workTime.Current)
            {
                yield return null;
                UpdateProgress(Time.deltaTime);
            }

            Complete();
        }

        private void UpdateProgress(float deltaTime)
        {
            currentTime += deltaTime;
            var progress = currentTime / workTime.Current;
            engine.Current.progress = progress;
        }

        private void Complete()
        {
            var operation = engine.Current;
            operation.isCompleted = true;
            operation.progress = 1.0f;
            engine.Stop();
        }
    }
}