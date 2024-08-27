using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine
{
    [AddComponentMenu("GameEngine/World Camera/World Camera Billboard")]
    [ExecuteAlways]
    public sealed class WorldCameraBillboard : MonoBehaviour
    {
        [SerializeField]
        private bool lookAtStart;
    
        private void Start()
        {
            if (lookAtStart)
            {
                LookAtCamera();                
            }
        }
        
        [Button]
        public void LookAtCamera()
        {
            var rootPosition = transform.position;
            var instance = WorldCamera.Instance;
            if (ReferenceEquals(instance, null))
            {
                return;
            }

            var cameraRotation = instance.transform.rotation;
            var cameraVector = cameraRotation * Vector3.forward;
            var worldUp = cameraRotation * Vector3.up;
            transform.LookAt(rootPosition + cameraVector, worldUp);
        }
    }
}