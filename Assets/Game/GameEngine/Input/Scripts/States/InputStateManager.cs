using Elementary;
using GameSystem;

namespace Game.GameEngine
{
    public sealed class InputStateManager : StateMachine<InputStateId>,
        IGameStartElement,
        IGameFinishElement
    {
        void IGameStartElement.StartGame()
        {
            Enter();
        }
        
        void IGameFinishElement.FinishGame()
        {
            Exit();
        }
    }
}