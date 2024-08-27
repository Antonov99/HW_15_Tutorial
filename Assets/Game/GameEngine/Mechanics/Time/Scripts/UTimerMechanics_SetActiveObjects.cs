using UnityEngine;

namespace Game.GameEngine
{
    [AddComponentMenu("GameEngine/Mechanics/Time/Timer Mechanics «Set Active Game Objects»")]
    public sealed class UTimerMechanics_SetActiveObjects : UTimerMechanics
    {
        [SerializeField]
        public bool setActive = true;

        [Space]
        [SerializeField]
        public GameObject[] objects;
        
        private void Awake()
        {
            SetActive(timer.IsPlaying);
        }

        protected override void OnTimerStarted()
        {
            SetActive(setActive);
        }

        protected override void OnTimerFinished()
        {
            SetActive(!setActive);
        }

        private void SetActive(bool isActive)
        {
            for (int i = 0, count = objects.Length; i < count; i++)
            {
                var obj = objects[i];
                obj.SetActive(isActive);
            }
        }
    }
}