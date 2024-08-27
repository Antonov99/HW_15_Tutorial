using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Hit Points/Hit Points Bar")]
    public sealed class HitPointsBar : MonoBehaviour
    {
        [SerializeField]
        private GameObject root;

        [SerializeField]
        private Image progressBar;

        [SerializeField]
        private TextMeshProUGUI text;

        public void SetHitPoints(int current, int max)
        {
            text.text = $"{current}/{max}";
            progressBar.fillAmount = (float) current / max;
        }

        public void SetVisible(bool isVisible)
        {
            root.SetActive(isVisible);
        }

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            root = gameObject;
        }
#endif
    }
}