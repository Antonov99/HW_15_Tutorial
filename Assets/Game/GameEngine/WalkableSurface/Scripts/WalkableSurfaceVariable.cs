using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class WalkableSurfaceVariable : MonoBehaviour
    {
        [ReadOnly]
        [ShowInInspector]
        public bool IsSurfaceExists
        {
            get { return surfaceExists; }
        }

        [ReadOnly]
        [ShowInInspector]
        public IWalkableSurface Surface
        {
            get { return surface; }
        }

        private IWalkableSurface surface;

        private bool surfaceExists;

        public void SetSurface(IWalkableSurface surface)
        {
            this.surface = surface;
            surfaceExists = true;
        }

        public void ResetSurface()
        {
            surface = null;
            surfaceExists = false;
        }
    }
}