using DG.Tweening;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class UIParryAnimatorMono : MonoBehaviour
    {
        [SerializeField]
        private RectTransform moveTransform;

        private Tween parryTween;
        
        private void OnEnable()
        {
            parryTween = UIAnimations.AnimateParry(moveTransform);
        }

        private void OnDisable()
        {
            parryTween.Kill();
        }
    }
}