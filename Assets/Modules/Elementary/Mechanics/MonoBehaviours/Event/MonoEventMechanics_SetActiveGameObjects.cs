using System.Collections;
using UnityEngine;

namespace Elementary
{
    [AddComponentMenu("Elementary/Mechanics/Event Mechanics «Set Active Game Objects»")]
    public sealed class MonoEventMechanics_SetActiveGameObjects : MonoEventMechanics
    {
        [Space]
        [SerializeField]
        private float hideDelay = 1.25f;
        
        [SerializeField]
        private bool setActive = true;
        
        [SerializeField]
        private GameObject[] gameObjects;

        protected override void OnEvent()
        {
            StartCoroutine(EnableObjects());
        }

        private IEnumerator EnableObjects()
        {
            yield return new WaitForSeconds(hideDelay);
            for (int i = 0, count = gameObjects.Length; i < count; i++)
            {
                var visual = gameObjects[i];
                visual.SetActive(setActive);
            }
        }
    }
}