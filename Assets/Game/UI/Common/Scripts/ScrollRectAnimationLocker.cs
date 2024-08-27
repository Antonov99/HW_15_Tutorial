#if UNITY_EDITOR

using Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class ScrollRectAnimationLocker : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect rect;
        
        [SerializeField]
        private WindowAnimator animator;

        private void OnEnable()
        {
            animator.OnShowStarted += OnAnimationStarted;
            animator.OnHideStarted += OnAnimationStarted;
            animator.OnShowFinished += OnAnimationFinished;
            animator.OnHideFinished += OnAnimationFinished;
        }

        private void OnDisable()
        {
            animator.OnShowStarted -= OnAnimationStarted;
            animator.OnHideStarted -= OnAnimationStarted;
            animator.OnShowFinished -= OnAnimationFinished;
            animator.OnHideFinished -= OnAnimationFinished;
        }

        private void OnAnimationStarted()
        {
            rect.enabled = false;
        }

        private void OnAnimationFinished()
        {
            rect.enabled = true;
        }

        private void Reset()
        {
            rect = GetComponent<ScrollRect>();
        }
    }
}

#endif