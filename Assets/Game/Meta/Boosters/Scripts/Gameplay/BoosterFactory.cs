using GameSystem;

namespace Game.Meta
{
    public sealed class BoosterFactory
    {
        [GameInject]
        private GameContext gameContext;
        
        public Booster CreateBooster(BoosterConfig config)
        {
            var booster = config.InstantiateBooster();
            GameInjector.Inject(gameContext, booster);
            return booster;
        }
    }
}