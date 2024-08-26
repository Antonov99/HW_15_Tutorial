using Lessons.Entities;
using Lessons.Game;
using Lessons.Game.Handlers;
using Lessons.Game.Handlers.Effects;
using Lessons.Game.Services;
using Lessons.Level;
using VContainer;
using VContainer.Unity;

namespace Lessons.Utils
{
    public sealed class SceneScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureLevel(builder);
            ConfigurePlayer(builder);
            ConfigureControllers(builder);

            builder.RegisterComponentInHierarchy<EntityInstaller>();
        }

        private void ConfigureLevel(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<TileMap>();
            builder.Register<EntityMap>(Lifetime.Singleton);
            builder.Register<LevelMap>(Lifetime.Singleton);
        }

        private void ConfigurePlayer(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<KeyboardInput>();
            builder.RegisterComponentInHierarchy<PlayerService>();
            builder.RegisterEntryPoint<PlayerController>();
        }

        private void ConfigureControllers(IContainerBuilder builder)
        {
            builder.Register<EventBus>(Lifetime.Singleton).AsSelf();
            
            builder.RegisterEntryPoint<ApplyDirectionHandler>();
            builder.RegisterEntryPoint<AttackHandler>();
            builder.RegisterEntryPoint<DealDamageHandler>();
            builder.RegisterEntryPoint<DestroyHandler>();
            builder.RegisterEntryPoint<MoveHandler>();
            
            builder.RegisterEntryPoint<ForceDirectionHandler>();
            
            builder.RegisterEntryPoint<DealDamageEffectHandler>();
            builder.RegisterEntryPoint<PushEffectHandler>();
        }
    }
}