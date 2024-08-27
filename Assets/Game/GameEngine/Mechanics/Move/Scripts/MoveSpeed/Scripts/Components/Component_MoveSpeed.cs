using System;
using Elementary;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_MoveSpeed : 
        IComponent_GetMoveSpeed,
        IComponent_SetMoveSpeed,
        IComponent_OnMoveSpeedChanged
    {
        public event Action<float> OnSpeedChanged
        {
            add { moveSpeed.OnValueChanged += value; }
            remove { moveSpeed.OnValueChanged -= value; }
        }

        public float Speed
        {
            get { return moveSpeed.Current; }
        }

        private readonly IVariable<float> moveSpeed;

        public Component_MoveSpeed(IVariable<float> moveSpeed)
        {
            this.moveSpeed = moveSpeed;
        }

        public void SetSpeed(float speed)
        {
            moveSpeed.Current = speed;
        }
    }
}