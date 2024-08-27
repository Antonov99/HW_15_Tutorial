using System;
using DG.Tweening;
using Declarative;
using UnityEngine;

namespace Game.GameEngine
{
    [Serializable]
    public sealed class UIParryMechanics :
        IEnableListener,
        IDisableListener
    {
        public RectTransform moveTransform;

        private Tween parryTween;

        void IEnableListener.OnEnable()
        {
            parryTween = UIAnimations.AnimateParry(moveTransform);
        }

        void IDisableListener.OnDisable()
        {
            parryTween.Kill();
        }
    }
}