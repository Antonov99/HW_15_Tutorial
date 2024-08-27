using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputModule
{
    public sealed class JoystickInput : MonoBehaviour
    {
        private const int MOUSE_BUTTON = 0;

        private const float MIN_MAGNITUDE = 0.05f;

        public event Action<Vector2> OnPositionStarted;
        
        public event Action<Vector2> OnPositionMoved;

        public event Action<Vector2> OnDirectionMoved;

        public event Action<Vector2> OnPositionEnded;
        
        public event Action OnCanceled;

        /// <summary>
        ///     <para>State.</para>
        /// </summary>
        private bool isHoldStarted;

        private bool isMoveStarted;

        private Vector2 centerScreenPosition;

        private EventSystem eventSystem;

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


#if UNITY_EDITOR
        private void UpdateMouse()
        {
            if (Input.GetMouseButtonDown(MOUSE_BUTTON) && !IsPointerOverGameObject())
            {
                StartInput(Input.mousePosition);
            }
            else if (isHoldStarted && Input.GetMouseButton(MOUSE_BUTTON))
            {
                ProcessMove(Input.mousePosition);
            }
            else if (isHoldStarted && Input.GetMouseButtonUp(MOUSE_BUTTON))
            {
                EndInput(Input.mousePosition);
            }
        }
        
        private bool IsPointerOverGameObject()
        {
            if (eventSystem == null)
            {
                return false;
            }

            return eventSystem.IsPointerOverGameObject();
        }
#else
        private void UpdateTouch()
        {
            var touchCount = Input.touchCount;
            if (touchCount < 1)
            {
                return;
            }

            var touch = Input.GetTouch(0);
            var touchPhase = touch.phase;
            if (touchPhase == TouchPhase.Began && !this.IsPointerOverGameObject(touch.fingerId))
            {
                this.StartInput(touch.position);
            }
            else if (this.isHoldStarted)
            {
                this.ProcessMove(touch.position);
            }
            else if (this.isHoldStarted && (touchPhase == TouchPhase.Canceled || touchPhase == TouchPhase.Ended))
            {
                this.EndInput(touch.position);
            }
        }
        
        private bool IsPointerOverGameObject(int fingerId)
        {
            if (this.eventSystem == null)
            {
                return false;
            }

            return this.eventSystem.IsPointerOverGameObject(fingerId);
        }
#endif

        private void StartInput(Vector2 inputPosition)
        {
            isHoldStarted = true;
            centerScreenPosition = inputPosition;
            OnPositionStarted?.Invoke(inputPosition);
        }

        private void ProcessMove(Vector2 inputPosition)
        {
            var screenVector = inputPosition - centerScreenPosition;
            if (isMoveStarted || screenVector.magnitude > MIN_MAGNITUDE)
            {
                isMoveStarted = true;
                OnPositionMoved?.Invoke(inputPosition);
                OnDirectionMoved?.Invoke(screenVector.normalized);
            }
        }

        private void EndInput(Vector2 inputPosition)
        {
            isMoveStarted = false;
            isHoldStarted = false;
            OnPositionEnded?.Invoke(inputPosition);
        }

        public void CancelInput()
        {
            if (isHoldStarted)
            {
                isMoveStarted = false;
                isHoldStarted = false;
                OnCanceled?.Invoke();
            }
        }
    }
}