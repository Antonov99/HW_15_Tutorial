using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Level/Level Visual «Game Object»")]
    public sealed class LevelVisual_GameObject : LevelVisualBase
    {
        [SerializeField]
        private GameObject root;

        public override void Activate()
        {
            root.SetActive(true);
        }

        public override void SetActive(bool isVisible)
        {
            root.SetActive(isVisible);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            root = gameObject;
        }
#endif
    }
}