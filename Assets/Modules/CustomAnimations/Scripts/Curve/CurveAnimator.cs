using System;
using System.Collections;
using UnityEngine;

namespace CustomAnimations
{
    public abstract class CurveAnimator<T> : MonoBehaviour
    {
        protected abstract T DefaultValue { get; }
        
        [SerializeField]
        private CurveAnimation config;
        
        private Coroutine animationCoroutine;

        protected abstract Func<float, T> SumFunction { get; }

        protected abstract Func<float, T> MultiplyFunction { get; }

        public void SetAnimation(CurveAnimation animation)
        {
            config = animation;
        }

        public void Play()
        {
            ResetState();
            animationCoroutine = StartCoroutine(AnimateRoutine());
        }

        public void Stop()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        public void ResetState()
        {
            Stop();
            SetValue(DefaultValue);
        }

        protected abstract void SetValue(T result);

        private IEnumerator AnimateRoutine()
        {
            Func<float, T> function;
            if (config.functionType == CurveFunctionType.SUM)
            {
                function = SumFunction;
            }
            else
            {
                function = MultiplyFunction;
            }

            const float end = 1.0f;
            var progress = 0f;
            var dProgress = Time.deltaTime / config.duration;
            var curve = config.curve;

            while (progress < end)
            {
                progress = Mathf.Min(progress + dProgress, end);
                var curveValue = curve.Evaluate(progress);
                var value = function.Invoke(curveValue);
                SetValue(value);
                yield return null;
            }
        }
    }
}