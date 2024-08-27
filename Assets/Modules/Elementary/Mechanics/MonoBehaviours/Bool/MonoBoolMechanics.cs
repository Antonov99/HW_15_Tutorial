using UnityEngine;

namespace Elementary
{
    public abstract class MonoBoolMechanics : MonoBehaviour
    {
        [SerializeField]
        private MonoBoolVariable toggle;

        protected virtual void Awake()
        {
            SetEnable(toggle.Current);
        }

        protected virtual void OnEnable()
        {
            toggle.OnValueChanged += SetEnable;
        }

        protected virtual void OnDisable()
        {
            toggle.OnValueChanged -= SetEnable;
        }

        protected abstract void SetEnable(bool isEnable);
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (toggle != null)
            {
                toggle.OnValueChanged -= SetEnable;
                toggle.OnValueChanged += SetEnable;
                SetEnable(toggle.Current);
            }
        }
#endif
    }
}