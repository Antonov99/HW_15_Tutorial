using Entities;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public abstract class UEnableObserver : MonoBehaviour
    {
        [SerializeField]
        public MonoEntity entity;

        protected virtual void Awake()
        {
            var isEnable = entity.Get<IComponent_Enable>().IsEnable;
            SetEnable(isEnable);
        }

        protected virtual void OnEnable()
        {
            entity.Get<IComponent_Enable>().OnEnabled += SetEnable;
        }

        protected virtual void OnDisable()
        {
            entity.Get<IComponent_Enable>().OnEnabled -= SetEnable;
        }

        protected abstract void SetEnable(bool isEnable);
    }
}