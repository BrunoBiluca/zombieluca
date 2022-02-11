namespace Assets.GameAssets.Items.Tests
{
    public interface IConsumableItemFactory
    {
        ICollisionObject Create();

        ConsumableItem GetConsumableItem();
        void Destroy();
    }
}