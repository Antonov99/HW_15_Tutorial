using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Mechanics/Bool Mechanics «Set Active Game Objects»")]
    public sealed class MonoBoolMechanics_SetActiveGameObjects : MonoBoolMechanics
    {
        [Space]
        [SerializeField]
        private GameObject[] gameObjects;

        protected override void SetEnable(bool isEnable)
        {
            for (int i = 0, count = gameObjects.Length; i < count; i++)
            {
                var gameObject = gameObjects[i];
                gameObject.SetActive(isEnable);
            }
        }
    }
}