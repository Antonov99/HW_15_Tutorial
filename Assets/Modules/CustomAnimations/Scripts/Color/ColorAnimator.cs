using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAnimations
{
    [AddComponentMenu("CustomAnimations/Color Animator")]
    public sealed class ColorAnimator : MonoBehaviour
    {
        [SerializeField]
        private Graphic[] graphics;

        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private ColorAnimation config;

        private Coroutine animationCoroutine;

        public void SetAnimation(ColorAnimation animation)
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
            SetColor(defaultColor);
        }

        private IEnumerator AnimateRoutine()
        {
            const float end = 1.0f;
            var progress = 0f;
            var dProgress = Time.deltaTime / config.duration;
            var gradient = config.colorGradient;

            while (progress < end)
            {
                progress = Mathf.Min(progress + dProgress, end);
                var color = gradient.Evaluate(progress);
                SetColor(color);
                yield return null;
            }
        }
        
        private void SetColor(Color color)
        {
            foreach (var graphic in graphics)
            {
                graphic.color = color;
            }
        }
    }
}