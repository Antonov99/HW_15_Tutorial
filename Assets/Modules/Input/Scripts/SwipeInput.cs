using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputModule
{
    public sealed class SwipeInput : MonoBehaviour
    {
        public event Action<Vector2> OnPositionStarted;
        
        public event Action<Vector2> OnPositionEnded;
        
        public event Action OnCanceled;

        [SerializeField]
        private float minSwipe = 50;

        private Vector2 startPosition;

        private bool isSwiping;

        private EventSystem eventSystem;

        #region Lifecycle

        private void Awake()
        {
            eventSystem = EventSystem.current;
        }

        private void Update()
        {
#if UNITY_EDITOR
            UpdateMouse();
#else
            this.UpdateTouch();
#endif
        }

        #endregion

#if UNITY_EDITOR
        private void UpdateMouse()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject())
            {
                StartInput(Input.mousePosition);
            }
            else if (isSwiping && Input.GetMouseButtonUp(0))
            {
                EndInput(Input.mousePosition);
            }
        }
        
        private bool IsPointerOverGameObject()
        {
            if (ReferenceEquals(eventSystem, null))
            {
                return false;
            }

            return eventSystem.IsPointerOverGameObject();
        }
#else
        private void UpdateTouch()
        {
            var touchCount = Input.touchCount;
            if (touchCount != 1)
            {
                return;
            }

            var touch = Input.GetTouch(0);
            var touchPhase = touch.phase;
            if (touchPhase == TouchPhase.Began && !this.IsPointerOverGameObject(touch.fingerId))
            {
                this.StartInput(touch.position);
            }
            else if (this.isSwiping && (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled))
            {
                this.EndInput(touch.position);
            }
        }
        
        private bool IsPointerOverGameObject(int fingerId)
        {
            if (ReferenceEquals(this.eventSystem, null))
            {
                return false;
            }

            return this.eventSystem.IsPointerOverGameObject(fingerId);
        }
#endif
        
        private void StartInput(Vector3 inputPosition)
        {
            startPosition = inputPosition;
            isSwiping = true;
            OnPositionStarted?.Invoke(inputPosition);
        }

        private void EndInput(Vector2 inputPosition)
        {
            var swipeVector = inputPosition - startPosition;
            if (swipeVector.sqrMagnitude >= minSwipe * 2)
            {
                OnPositionEnded?.Invoke(inputPosition);
            }
            
            isSwiping = false;
        }

        public void Cancel()
        {
            if (isSwiping)
            {
                isSwiping = false;
                OnCanceled?.Invoke();
            }
        }
    }
}