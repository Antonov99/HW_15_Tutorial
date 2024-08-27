using System;
using System.Collections;
using System.Threading.Tasks;
using Entities;
using GameSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Game.Tutorial
{
    [Serializable]
    public sealed class KillEnemyManager
    {
        private GameContext gameContext;
        
        [SerializeField]
        private AssetReference enemyPrefab;
        
        [SerializeField]
        private Transform worldTransform;

        [SerializeField]
        private Transform spawnPoint;
        
        [SerializeField]
        private float destroyDelay = 1.25f;

        public void Construct(GameContext context)
        {
            gameContext = context;
        }

        public async Task<MonoEntity> SpawnEnemy()
        {
            var handle = this.enemyPrefab.LoadAssetAsync<GameObject>();
            await handle.Task;
            var enemyPrefab = handle.Result.GetComponent<MonoEntity>();

            var enemy = Object.Instantiate(
                enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                worldTransform
            );

            var gameElement = enemy.GetComponent<IGameElement>();
            gameContext.RegisterElement(gameElement);

            return enemy;
        }

        public IEnumerator DestroyEnemy(MonoEntity entity)
        {
            var gameElement = entity.GetComponent<IGameElement>();
            gameContext.UnregisterElement(gameElement);
            
            yield return new WaitForSecondsRealtime(destroyDelay);
            Object.Destroy(entity.gameObject);
        }
    }
}