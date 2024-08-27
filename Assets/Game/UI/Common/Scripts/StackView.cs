using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class StackView : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private GameObject stackContainer;

        [SerializeField]
        private TextMeshProUGUI stackText;
    
        public void SetVisible(bool isVisible)
        {
            stackContainer.SetActive(isVisible);
        }

        public void SetAmount(int count, int max)
        {
            stackText.text = $"{count}/{max}";
        }
    }
}