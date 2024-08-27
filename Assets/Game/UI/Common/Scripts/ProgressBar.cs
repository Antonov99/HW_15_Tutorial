using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private GameObject root;

        [FormerlySerializedAs("progress")]
        [SerializeField]
        private Image fillImage;

        [Space]
        [SerializeField]
        private bool hasMask;

        [ShowIf("hasMask")]
        [OptionalField]
        [SerializeField]
        private Image maskImage;

        public void SetVisible(bool isVisible)
        {
            root.SetActive(isVisible);
        }

        public void SetProgress(float progress)
        {
            if (hasMask)
            {
                maskImage.fillAmount = progress;
            }
            else
            {
                fillImage.fillAmount = progress;
            }
        }

        public void SetColor(Color color)
        {
            fillImage.color = color;
        }
    }
}