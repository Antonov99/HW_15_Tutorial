using System;
using Elementary;
using Game.GameEngine.GameResources;
using UnityEngine;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Harvest Resource/Harvest Resource State «Set Active Game Objects»")]
    public sealed class UHarvestResourceState_SetActiveGameObjects : MonoState
    {
        [SerializeField]
        private bool disableOnAwake;

        [Space]
        [SerializeField]
        private UHarvestResourceOperator engine;

        [Space]
        [SerializeField]
        private GameObjectGroup[] objectGroups;

        private GameObject[] currentGameObjects; 

        public override void Enter()
        {
            var resourceType = engine
                .Current
                .targetResource
                .Get<IComponent_GetResourceType>()
                .Type;

            currentGameObjects = GetGameObjects(resourceType);
            EnableGameObjects(currentGameObjects, true);
        }

        public override void Exit()
        {
            EnableGameObjects(currentGameObjects, false);
        }

        private void Awake()
        {
            if (disableOnAwake)
            {
                DisableAllGameObjects();
            }
        }

        private void DisableAllGameObjects()
        {
            for (int i = 0, count = objectGroups.Length; i < count; i++)
            {
                var group = objectGroups[i];
                var gameObjects = group.objects;
                EnableGameObjects(gameObjects, false);
            }
        }

        private void EnableGameObjects(GameObject[] gameObjects, bool isEnable)
        {
            for (int i = 0, count = gameObjects.Length; i < count; i++)
            {
                var gameObject = gameObjects[i];
                gameObject.SetActive(isEnable);
            }
        }

        private GameObject[] GetGameObjects(ResourceType resourceType)
        {
            for (int i = 0, count = objectGroups.Length; i < count; i++)
            {
                var objectGroup = objectGroups[i];
                if (objectGroup.resourceType == resourceType)
                {
                    return objectGroup.objects;
                }
            }

            throw new Exception($"GameObject group is not found {resourceType}!");
        }

        [Serializable]
        private sealed class GameObjectGroup
        {
            [SerializeField]
            public ResourceType resourceType;

            [SerializeField]
            public GameObject[] objects = new GameObject[0];
        }
    }
}