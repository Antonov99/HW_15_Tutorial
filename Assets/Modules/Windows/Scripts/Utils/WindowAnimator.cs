using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Windows
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    [AddComponentMenu(Extensions.MENU_PATH + "Window Animator")]
    public sealed class WindowAnimator : MonoBehaviour
    {
        public event Action OnShowStarted;

        public event Action OnShowFinished;

        public event Action OnHideStarted;

        public event Action OnHideFinished;

        private const string SHOW_ANIMATION_NAME = "Show";

        private const string HIDE_ANIMATION_NAME = "Hide";

        [SerializeField]
        private Animator animator;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onShown;

        [SerializeField]
        private UnityEvent onHidden;

        #region Events

        public void Show()
        {
            OnShowStarted?.Invoke();
            animator.Play(SHOW_ANIMATION_NAME, -1, 0);
        }

        public void Hide()
        {
            OnHideStarted?.Invoke();
            animator.Play(HIDE_ANIMATION_NAME, -1, 0);
        }

        [UsedImplicitly]
        private void OnShown()
        {
            onShown?.Invoke();
            OnShowFinished?.Invoke();
        }

        [UsedImplicitly]
        private void OnHidden()
        {
            onHidden?.Invoke();
            OnHideFinished?.Invoke();
        }

        #endregion

#if UNITY_EDITOR
        private void Reset()
        {
            animator = GetComponent<Animator>();
        }
#endif
    }
}