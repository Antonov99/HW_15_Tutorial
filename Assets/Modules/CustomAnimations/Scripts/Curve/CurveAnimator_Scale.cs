using System;
using UnityEngine;

namespace CustomAnimations
{
    [AddComponentMenu("CustomAnimations/Curve Animator «Scale»")]
    public sealed class CurveAnimator_Scale : CurveAnimator<Vector3>
    {
        [Space]
        [Header("Scale")]
        [SerializeField]
        private Transform root;

        private Vector3 defaultScale;

        protected override Vector3 DefaultValue
        {
            get { return defaultScale; }
        }

        protected override Func<float, Vector3> SumFunction
        {
            get { return sumFunction; }
        }

        protected override Func<float, Vector3> MultiplyFunction
        {
            get { return multiplyFunction; }
        }

        private Func<float, Vector3> sumFunction;

        private Func<float, Vector3> multiplyFunction;

        protected override void SetValue(Vector3 result)
        {
            root.localScale = result;
        }

        #region Lifecycle

        private void Awake()
        {
            defaultScale = root.localScale;
            sumFunction = term => defaultScale + new Vector3(term, term, term);
            multiplyFunction = multiplier => defaultScale * (1.0f + multiplier); 
        }

        #endregion
    }
}