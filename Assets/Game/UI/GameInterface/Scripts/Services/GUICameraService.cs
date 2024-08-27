using UnityEngine;

namespace Game.GameEngine.GUI
{
    public sealed class GUICameraService : MonoBehaviour
    {
        public Camera Camera
        {
            get { return camera; }
        }

        [SerializeField]
        private new Camera camera;
    }
}