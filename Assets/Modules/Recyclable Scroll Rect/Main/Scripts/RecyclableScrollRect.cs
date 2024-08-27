using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PolyAndCode.UI
{
    public sealed class RecyclableScrollRect : ScrollRect
    {
        public int Segments
        {
            set { _segments = Math.Max(value, 2); }
            get { return _segments; }
        }

        [SerializeField]
        private bool IsGrid;

        [SerializeField]
        private DirectionType Direction;

        [SerializeField]
        private int _segments;

        private IScroller scroller;

        private Vector2 _prevAnchoredPos;

        protected override void Start()
        {
            vertical = Direction == DirectionType.Vertical;
            horizontal = Direction == DirectionType.Horizontal;
        }

        public void Initialize<T>(IDataAdapter<T> adapter) where T : Component
        {
            _prevAnchoredPos = content.anchoredPosition;
            onValueChanged.RemoveListener(OnPositionChanged);
            StartCoroutine(InitializeRoutine(adapter));
        }

        private IEnumerator InitializeRoutine<T>(IDataAdapter<T> adapter) where T : Component
        {
            if (Direction == DirectionType.Vertical)
            {
                yield return InitializeAsVertical(adapter);
            }
            else if (Direction == DirectionType.Horizontal)
            {
                yield return InitializeAsHorizontal(adapter);
            }

            onValueChanged.AddListener(OnPositionChanged);
        }

        private IEnumerator InitializeAsVertical<T>(IDataAdapter<T> adapter) where T : Component
        {
            var scroller = new VerticalScroller<T>(viewport, content, adapter, IsGrid, Segments);
            yield return scroller.Start();
            this.scroller = scroller;
        }

        private IEnumerator InitializeAsHorizontal<T>(IDataAdapter<T> adapter) where T : Component
        {
            var scroller = new HorizontalScroller<T>(viewport, content, adapter, IsGrid, Segments);
            yield return scroller.Start();
            this.scroller = scroller;
        }

        private void OnPositionChanged(Vector2 normalizedPos)
        {
            var direction = content.anchoredPosition - _prevAnchoredPos;
            m_ContentStartPosition += scroller.DoScroll(direction);
            _prevAnchoredPos = content.anchoredPosition;
        }

        public enum DirectionType
        {
            Vertical,
            Horizontal
        }
    }
}