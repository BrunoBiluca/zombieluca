using Assets.GameAssets.AmmoStorage;
using Moq;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class StorageFactory : IItemUserFactory
    {
        public MonoBehaviour Create()
        {
            return new GameObject("storage")
                .AddComponent<AmmoStorageMonoBehaviour>()
                .Setup(new Mock<IAmmoStorage>().Object);
        }
    }
}