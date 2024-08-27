using System;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Move/Component «Move In Direction»")]
    public sealed class UComponent_MoveInDirection : MonoBehaviour,
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

        [SerializeField]
        private UMoveInDirectionMotor engine;

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