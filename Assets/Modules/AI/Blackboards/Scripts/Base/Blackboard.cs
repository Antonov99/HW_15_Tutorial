using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace AI.Blackboards
{
    [Serializable]
    public sealed class Blackboard : IBlackboard
    {
        public event Action<string, object> OnVariableAdded;

        public event Action<string, object> OnVariableChanged;

        public event Action<string, object> OnVariableRemoved;

        public object this[string key]
        {
            get => variables[key];
            set => ChangeVariable(key, value);
        }

        [ShowInInspector, ReadOnly]
        private readonly Dictionary<string, object> variables = new ();

        public IEnumerable<KeyValuePair<string, object>> GetVariables()
        {
            foreach (var variable in variables)
            {
                yield return variable;
            }
        }

        public bool TryGetVariable<T>(string key, out T value)
        {
            if (variables.TryGetValue(key, out var result))
            {
                value = (T) result;
                return true;
            }

            value = default;
            return false;
        }

        public bool HasVariable(string key)
        {
            return variables.ContainsKey(key);
        }

        public T GetVariable<T>(string key)
        {
            if (!variables.TryGetValue(key, out var result))
            {
                throw new Exception($"{key} value is not found!");
            }

            return (T) result;
        }

        public void AddVariable(string key, object value)
        {
            if (variables.ContainsKey(key))
            {
                throw new Exception($"Variable {key} is already added!");
            }

            variables.Add(key, value);
            OnVariableAdded?.Invoke(key, value);
        }

        public void ChangeVariable(string key, object value)
        {
            if (!variables.ContainsKey(key))
            {
                throw new Exception($"Variable {key} is not found!");
            }

            variables[key] = value;
            OnVariableChanged?.Invoke(key, value);
        }

        public void RemoveVariable(string key)
        {
            if (variables.TryGetValue(key, out var value))
            {
                variables.Remove(key);
                OnVariableRemoved?.Invoke(key, value);
            }
        }

        public void Clear()
        {
            foreach (var (key, value) in variables)
            {
                OnVariableRemoved?.Invoke(key, value);
            }
            
            variables.Clear();
        }
    }
}