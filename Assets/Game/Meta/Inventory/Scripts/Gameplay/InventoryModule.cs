using Game.GameEngine.InventorySystem;
using Game.GameEngine.Products;
using Game.Gameplay.Hero;
using GameSystem;
using Sirenix.OdinInspector;

namespace Game.Meta
{
    public sealed class InventoryModule : GameModule
    {
        private readonly StackableInventory inventory = new();
        
        [GameService]
        [ReadOnly, ShowInInspector]
        private InventoryService service = new();

        [GameService]
        [ShowInInspector]
        private InventoryItemConsumer itemConsumer = new();

        [GameService]
        [ShowInInspector]
        private InventoryItemCrafter itemCrafter = new();

        [GameElement]
        [ReadOnly, ShowInInspector]
        private InventoryItemEffectHandler effectsObserver = new();

        public override void ConstructGame(GameContext context)
        {
            service.Setup(inventory);
            itemConsumer.SetInventory(inventory);
            itemCrafter.SetInventory(inventory);

            InstallEffectObserver(context);
            InstallConsumeHealingKit(context);
            InstallProductBuyKit(context);
        }

        private void InstallEffectObserver(GameContext context)
        {
            var heroService = context.GetService<HeroService>();
            effectsObserver.Construct(heroService);
            effectsObserver.SetInventory(inventory);
        }

        private void InstallConsumeHealingKit(GameContext context)
        {
            var heroService = context.GetService<HeroService>();
            itemConsumer.AddHandler(new HealingInventoryItemConsumeHandler(heroService));
        }

        private void InstallProductBuyKit(GameContext context)
        {
            var buySystem = context.GetService<ProductBuyer>();
            buySystem.AddCompletor(new InventoryItemBuyCompletor(inventory));
        }
        
        [Title("Debug")]
        [Button]
        private void AddItems(InventoryItemConfig itemInfo, int count)
        {
            inventory.AddItemsByPrototype(itemInfo.Prototype, count);
        }

        [Button]
        private void RemoveItem(InventoryItemConfig itemInfo, int count)
        {
            inventory.RemoveItems(itemInfo.ItemName, count);
        }
    }
}