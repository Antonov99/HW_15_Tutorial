using UnityEngine;

namespace Game.Gameplay.Conveyors
{
    [AddComponentMenu("Gameplay/Conveyors/Conveyor Visual")]
    public sealed class ConveyorVisual : MonoBehaviour
    {
        private static readonly int STATE = Animator.StringToHash("State");

        private const int IDLE_ANIMATION = 0;

        private const int SAW_ANIMATION = 1;

        [Space]
        [SerializeField]
        private Animator workerAnimator;

        [SerializeField]
        private GameObject sawObject;

        [SerializeField]
        private GameObject woodObject;

        private void Awake()
        {
            sawObject.SetActive(false);
            woodObject.SetActive(false);
        }

        public void Play()
        {
            workerAnimator.SetInteger(STATE, SAW_ANIMATION);
            sawObject.SetActive(true);
            woodObject.SetActive(true);
        }

        public void Stop()
        {
            workerAnimator.SetInteger(STATE, IDLE_ANIMATION);
            sawObject.SetActive(false);
            woodObject.SetActive(false);
        }
    }
}