using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Transform/Component «Transform Engine»")]
    public sealed class UComponent_TransformEngine : MonoBehaviour,
        IComponent_GetPosition,
        IComponent_SetPosition,
        IComponent_GetRotation,
        IComponent_SetRotation,
        IComponent_LookAtPosition
    {
        public Vector3 Position
        {
            get { return engine.WorldPosition; }
        }

        public Quaternion Rotation
        {
            get { return engine.WorldRotation; }
        }

        [SerializeField]
        private UTransformEngine engine;

        public void SetPosition(Vector3 position)
        {
            engine.SetPosiiton(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            engine.SetRotation(rotation);
        }

        public void LookAtPosition(Vector3 position)
        {
            engine.LookAtPosition(position);
        }
    }
}