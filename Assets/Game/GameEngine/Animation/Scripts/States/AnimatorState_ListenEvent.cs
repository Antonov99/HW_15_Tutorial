using System;
using Elementary;

namespace Game.GameEngine.Animation
{
    public sealed class AnimatorState_ListenEvent : State
    {
        private AnimatorMachine animationSystem;

        private IAction action;

        private string[] animationEvents;

        public void ConstructAnimMachine(AnimatorMachine machine)
        {
            animationSystem = machine;
        }

        public void ConstructAnimEvents(params string[] animEvents)
        {
            animationEvents = animEvents;
        }

        public void ConstructAction(IAction action)
        {
            this.action = action;
        }

        public void ConstructAction(Action action)
        {
            this.action = new ActionDelegate(action);
        }

        public override void Enter()
        {
            animationSystem.OnStringReceived += OnAnimationEvent;
        }

        public override void Exit()
        {
            animationSystem.OnStringReceived -= OnAnimationEvent;
        }

        private void OnAnimationEvent(string message)
        {
            if (ContainsEvent(message))
            {
                action.Do();
            }
        }

        private bool ContainsEvent(string message)
        {
            for (int i = 0, count = animationEvents.Length; i < count; i++)
            {
                if (animationEvents[i] == message)
                {
                    return true;
                }
            }

            return false;
        }
    }
}