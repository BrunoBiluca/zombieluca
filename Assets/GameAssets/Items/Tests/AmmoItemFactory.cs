using Assets.UnityFoundation.UnityAdapter;
using Moq;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class AmmoItemFactory : IConsumableItemFactory
    {
        private AmmoItem item;

        public ICollisionObject Create()
        {
            item = new AmmoItem(5);

            return new GameObject("ammo")
                .AddComponent<AmmoItemMonoBehaviour>()
                .Setup(item, new Mock<IAudioSource>().Object);
        }

        public ConsumableItem GetConsumableItem()
        {
            return item;
        }
    }
}