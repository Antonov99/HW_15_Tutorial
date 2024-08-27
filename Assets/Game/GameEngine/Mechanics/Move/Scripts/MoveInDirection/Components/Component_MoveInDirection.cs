using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [Serializable]
    public sealed class Component_MoveInDirection :
        IComponent_MoveInDirection,
        IComponent_IsMoving,
        IComponent_CanMoveInDirection,
        IComponent_OnMoveStarted,
        IComponent_OnMoveStopped
    {
        public event Action OnStarted
        {
            add { engine.OnStartMove += value; }
            remove { engine.OnStartMove -= value; }
        }

        public event Action OnStopped
        {
            add { engine.OnStopMove += value; }
            remove { engine.OnStopMove -= value; }
        }

        public bool IsMoving
        {
            get { return engine.IsMoving; }
        }

        private readonly IMoveInDirectionMotor engine;

        public Component_MoveInDirection(IMoveInDirectionMotor engine)
        {
            this.engine = engine;
        }

        public bool CanMove(Vector3 direction)
        {
            return engine.CanMove(direction);
        }

        public void Move(Vector3 direction)
        {
            engine.RequestMove(direction);
        }
    }
}