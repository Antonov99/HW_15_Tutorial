using TMPro;
using UnityEngine;

namespace Game.Tutorial
{
    public sealed class CongratulationsView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        public void Show(object args)
        {
            if (args is CongratulationsArgs infoArgs)
            {
                titleText.text = infoArgs.title;
                descriptionText.text = infoArgs.description;
            }
        }
    }
}