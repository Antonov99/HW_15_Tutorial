using Elementary;
using GameSystem;
using InputModule;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class InputModule : GameModule
    {
        [GameService, GameElement]
        [ShowInInspector]
        private readonly InputStateManager stateManager = new();

        [GameService]
        [SerializeField]
        private JoystickInput joystick;

        private void Awake()
        {
            joystick.enabled = false;
        }

        public override void ConstructGame(GameContext context)
        {
            stateManager.AddState(InputStateId.BASE, new InputState_Joystick(joystick));
            stateManager.AddState(InputStateId.LOCK, new State());
        }
    }
}