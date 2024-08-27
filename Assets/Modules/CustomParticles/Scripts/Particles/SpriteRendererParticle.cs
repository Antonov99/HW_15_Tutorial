using UnityEngine;

namespace CustomParticles
{
    public sealed class SpriteRendererParticle : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public void SetIcon(Sprite icon)
        {
            spriteRenderer.sprite = icon;
        }

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}