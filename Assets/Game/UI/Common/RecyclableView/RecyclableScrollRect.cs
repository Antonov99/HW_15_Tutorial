using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class RecyclableScrollRect : ScrollRect
    {
        public void MoveContentStartPosition(Vector2 vector)
        {
            m_ContentStartPosition += vector;
        }
    }
}