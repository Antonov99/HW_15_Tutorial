using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_Transform : 
        IComponent_GetPosition,
        IComponent_SetPosition,
        IComponent_GetRotation,
        IComponent_SetRotation
    {
        public Vector3 Position
        {
            get { return root.position; }
        }

        public Quaternion Rotation
        {
            get { return root.rotation; }
        }

        private readonly Transform root;

        public Component_Transform(Transform root)
        {
            this.root = root;
        }

        public void SetPosition(Vector3 position)
        {
            root.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            root.rotation = rotation;
        }
    }
}