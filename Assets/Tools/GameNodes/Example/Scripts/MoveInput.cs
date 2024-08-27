using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameNodes
{
    public interface IMoveInput
    {
        event Action<Vector2> OnMoved;
    }

    [Serializable]
    public sealed class MoveInput : IMoveInput, IGameUpdater
    {
        public event Action<Vector2> OnMoved;
        
        private InputSystem inputSystem;

        [ShowInInspector, ReadOnly]
        private bool enabled;

        [Space]
        [SerializeField]
        private KeyCode upCode;

        [SerializeField]
        private KeyCode downCode;

        [SerializeField]
        private KeyCode leftCode;

        [SerializeField]
        private KeyCode rightCode;

        [GameInit]
        public void Init(InputSystem inputSystem)
        {
            this.inputSystem = inputSystem;
        }
        
        [GameStart]
        public void Enable()
        {
            enabled = true;
        }

        [GameFinish]
        public void Disable()
        {
            enabled = false;
        }

        void IGameUpdater.Update(float deltaTime)
        {
            if (enabled)
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            if (inputSystem.GetKey(upCode))
            {
                OnMoved?.Invoke(Vector2.up);
            }
            else if (inputSystem.GetKey(downCode))
            {
                OnMoved?.Invoke(Vector2.down);
            }

            if (inputSystem.GetKey(leftCode))
            {
                OnMoved?.Invoke(Vector2.left);
            }
            else if (inputSystem.GetKey(rightCode))
            {
                OnMoved?.Invoke(Vector2.right);
            }
        }
    }
}