using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class HealItemFactory : IConsumableItemFactory
    {
        private HealItem item;

        public ICollisionObject Create()
        {
            item = new HealItem(10);

            return new GameObject("heal")
                .AddComponent<HealItemMonoBehaviour>()
                .Setup(item);
        }

        public ConsumableItem GetConsumableItem()
        {
            return item;
        }
    }
}