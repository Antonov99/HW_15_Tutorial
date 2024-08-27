using System.Collections.Generic;
using AI.Blackboards;

namespace Game.GameEngine.AI
{
    public sealed class Component_Blackboard : IComponent_Blackboard
    {
        private readonly IBlackboard blackboard;

        public Component_Blackboard(IBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public T GetVariable<T>(string key)
        {
            return blackboard.GetVariable<T>(key);
        }

        public IEnumerable<KeyValuePair<string, object>> GetVariables()
        {
            return blackboard.GetVariables();
        }

        public bool TryGetVariable<T>(string key, out T value)
        {
            return blackboard.TryGetVariable(key, out value);
        }

        public bool HasVariable(string key)
        {
            return blackboard.HasVariable(key);
        }

        public void AddVariable(string key, object value)
        {
            blackboard.AddVariable(key, value);
        }

        public void ChangeVariable(string key, object value)
        {
            blackboard.ChangeVariable(key, value);
        }

        public void RemoveVariable(string key)
        {
            blackboard.RemoveVariable(key);
        }

        public void Clear()
        {
            blackboard.Clear();
        }
    }
}