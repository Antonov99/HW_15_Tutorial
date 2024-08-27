using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Destroy/Destroy Mechanics «Play Animation»")]
    public sealed class UDestroyMechanics_PlayAnimation : UDestroyMechanics
    {
        [SerializeField]
        public Animator animator;

        [SerializeField]
        public string animationName = "Destroy";

        protected override void OnDestroyEvent(DestroyArgs destroyArgs)
        {
            animator.Play(animationName, -1, 0);
        }
    }
}