using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Destroy/Destroy Mechanics «Hide Game Objects»")]
    public sealed class UDestroyMechanics_HideGameObjects : UDestroyMechanics
    {
        [SerializeField]
        public GameObject[] gameObjects;
        
        protected override void OnDestroyEvent(DestroyArgs destroyArgs)
        {
            for (int i = 0, count = gameObjects.Length; i < count; i++)
            {
                var gameObject = gameObjects[i];
                gameObject.SetActive(false);
            }
        }
    }
}